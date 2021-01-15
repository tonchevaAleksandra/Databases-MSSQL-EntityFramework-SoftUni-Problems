using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> allEntitiies;

        private readonly List<T> added;

        private readonly List<T> removed;

        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();

            this.allEntitiies = CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities =>
            this.allEntitiies.AsReadOnly();

        public IReadOnlyCollection<T> Added =>
            this.added.AsReadOnly();

        public IReadOnlyCollection<T> Removed =>
            this.removed.AsReadOnly();

        public void Add(T item) =>
            this.added.Add(item);

        public void Remove(T item) =>
            this.removed.Add(item);

        private List<T> CloneEntities(IEnumerable<T> entities)
        {
            List<T> cloneEntities = new List<T>();

            PropertyInfo[] propertiesToClone = typeof(T)
                .GetProperties()
                .Where(pi => DbContext
                .AllowedSqlTypes
                .Contains(pi.PropertyType))
                .ToArray();

            foreach (T entity in entities)
            {
                T clone = Activator.CreateInstance<T>();

                foreach (PropertyInfo property in propertiesToClone)
                {
                    object value = property.GetValue(entity);
                    property.SetValue(clone, value);
                }

                cloneEntities.Add(clone);
            }

            return cloneEntities;
        }

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            List<T> modifiedEntities = new List<T>();

           PropertyInfo[] primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (T proxyEntity in this.allEntitiies)
            {
                object[] primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity)
                    .ToArray();

                T entity = dbSet
                    .Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e)
                    .SequenceEqual(primaryKeyValues));

                bool isModified = IsModified(proxyEntity, entity);

                if (isModified)
                {
                    modifiedEntities.Add(entity);
                }
            }

            return modifiedEntities;
        }

        private static bool IsModified(T proxyEntity, T entity)
        {
            PropertyInfo[] monitoredProperties = typeof(T)
                 .GetProperties()
                 .Where(pi => DbContext
                 .AllowedSqlTypes
                 .Contains(pi.PropertyType))
                 .ToArray();

            PropertyInfo[] modifiedProperties = monitoredProperties
                .Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(proxyEntity)))
                .ToArray();

            bool isModified = modifiedProperties.Any();

            return isModified;
        }

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T proxyEntity)
        {
            return primaryKeys
                .Select(pk => pk.GetValue(proxyEntity));
        }
    }
}