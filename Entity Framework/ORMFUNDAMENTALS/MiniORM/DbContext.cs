using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    public abstract class DbContext
    {
        private readonly DatabaseConnection connection;
        private readonly Dictionary<Type, PropertyInfo> dbSetProperties;

        internal static readonly Type[] AllowedSqlTypes =
        {
            typeof(string),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(bool),
            typeof(DateTime)
        };

        protected DbContext(string connectionString)
        {
            this.connection = new DatabaseConnection(connectionString);

            this.dbSetProperties = this.DiscoverDbSets();

            using (new ConnectionManager(connection))
            {
                this.InitializeDbSets();
            }

            this.MapAllRelations();
        }

        public void SaveChanges()
        {
            IEnumerable<object> dbSets = this.dbSetProperties
                .Select(pi => pi.Value.GetValue(this))
                .ToArray();

            foreach (IEnumerable<object> dbSet in dbSets)
            {
                object[] invalidEntities = dbSet
                    .Where(entity => !IsObjectValid(entity))
                    .ToArray();

                if (invalidEntities.Any())
                {
                    throw new InvalidOperationException($"{invalidEntities.Length} Invalid Entities found in {dbSet.GetType().Name}!");
                }
            }

            using (new ConnectionManager(connection))
            {
                using (SqlTransaction transaction = this.connection.StartTransaction())
                {
                    foreach (IEnumerable<object> dbSet in dbSets)
                    {
                        Type dbSetType = dbSet.GetType().GetGenericArguments().First();

                        var persistMethod = typeof(DbContext)
                            .GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(dbSetType);

                        try
                        {
                            persistMethod.Invoke(this, new object[] { dbSet });
                        }
                        catch (TargetInvocationException tie)
                        {
                            throw tie.InnerException;
                            throw;
                        }
                        catch (InvalidOperationException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    transaction.Commit();
                }
            }

        }

        private static bool IsObjectValid(object e)
        {
            ValidationContext validationContext = new ValidationContext(e);

            List<ValidationResult> validationErrors = new List<ValidationResult>();

            var validationResult = Validator.TryValidateObject(e, validationContext, validationErrors, true);

            return validationResult;
        }

        private void MapAllRelations()
        {
            foreach (var dbSetProperty in this.dbSetProperties)
            {
                Type dbSetType = dbSetProperty.Key;

                var mapRelationsGeneric = typeof(DbContext)
                    .GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                object dbSet = dbSetProperty.Value.GetValue(this);

                mapRelationsGeneric.Invoke(this, new[] { dbSet });
            }
        }

        private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);

            MapNavigationProperties(dbSet);

            PropertyInfo[] collections = entityType
                .GetProperties()
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToArray();

            foreach (PropertyInfo collection in collections)
            {
                Type collectionType = collection
                    .PropertyType
                    .GenericTypeArguments
                    .First();

                var mapCollectionMethod = typeof(DbContext)
                    .GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType, collectionType);

                mapCollectionMethod.Invoke(this, new object[] { dbSet, collection });
            }
        }

        private void MapCollection<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collectionProperty)
            where TDbSet : class, new()
            where TCollection : class, new()
        {
            Type entityType = typeof(TDbSet);
            Type collectionType = typeof(TCollection);

            PropertyInfo[] primaryKeys = collectionType
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            PropertyInfo primaryKey = primaryKeys.First();

            PropertyInfo foreignKey = entityType.GetProperties()
                .First(pi => pi.HasAttribute<KeyAttribute>());

            bool isManyToMany = primaryKeys.Length >= 2;

            if (isManyToMany)
            {
                primaryKey = collectionType.GetProperties()
                    .First(pi => collectionType.GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
                    .PropertyType == entityType);
            }

            var navigationDbSet = (DbSet<TCollection>)this.dbSetProperties[collectionType].GetValue(this);

            foreach (var entity in dbSet)
            {
                object primaryKeyValue = foreignKey.GetValue(entity);

                TCollection[] navigationEntities = navigationDbSet
                    .Where(navigationEntity => primaryKey.GetValue(navigationEntity).Equals(primaryKeyValue))
                    .ToArray();

                ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);
            }
        }

        private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);
            PropertyInfo[] foreignKeys = entityType
               .GetProperties()
               .Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
               .ToArray();

            foreach (PropertyInfo foreignKey in foreignKeys)
            {
                string navigationPropertyName = foreignKey
                    .GetCustomAttribute<ForeignKeyAttribute>().Name;

                PropertyInfo navigationProperty = entityType.GetProperty(navigationPropertyName);

                object navigationDbSet = this.dbSetProperties[navigationProperty.PropertyType]
                    .GetValue(this);

                PropertyInfo navigationPrimaryKey = navigationProperty
                    .PropertyType
                    .GetProperties()
                    .First(pi => pi.HasAttribute<KeyAttribute>());

                foreach (var entity in dbSet)
                {
                    object foreignKeyValue = foreignKey.GetValue(entity);

                    object navigationPropertyValue = ((IEnumerable<object>)navigationDbSet)
                        .First(currentNavigationProperty => navigationPrimaryKey
                        .GetValue(currentNavigationProperty)
                        .Equals(foreignKeyValue));

                    navigationProperty.SetValue(entity, navigationPropertyValue);

                }
            }
        }

        private void InitializeDbSets()
        {
            foreach (var dbset in this.dbSetProperties)
            {
                Type dbSetType = dbset.Key;
                PropertyInfo dbSetPropery = dbset.Value;

                var populatedDbSetGeneric = typeof(DbContext)
                    .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                populatedDbSetGeneric.Invoke(this, new object[] { dbSetPropery });
            }
        }

        private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
            where TEntity : class, new()
        {
            var entities = LoadTableEntities<TEntity>();

            var dbSetInstance = new DbSet<TEntity>(entities);
            ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
        }
        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class
        {
            Type table = typeof(TEntity);
            string[] columns = GetEntityColumnNames(table);
            string tableName = GetTableName(table);

            TEntity[] fetchedRows = this.connection.FetchResultSet<TEntity>(tableName, columns).ToArray();

            return fetchedRows;

        }

        private Dictionary<Type, PropertyInfo> DiscoverDbSets()
        {
            Dictionary<Type, PropertyInfo> dbSets = this
                 .GetType()
                 .GetProperties()
                 .Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                 .ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);

            return dbSets;
        }

        private void Persist<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            string tableName = GetTableName(typeof(TEntity));
            string[] columns = this.connection.FetchColumnNames(tableName).ToArray();

            if (dbSet.ChangeTracker.Added.Any())
            {
                this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
            }

            TEntity[] modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();

            if (modifiedEntities.Any())
            {
                this.connection.UpdateEntities(modifiedEntities, tableName, columns);
            }

            if (dbSet.ChangeTracker.Removed.Any())
            {
                this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
            }
        }

        private string GetTableName(Type tableType)
        {
            string tableName = ((TableAttribute)Attribute.GetCustomAttribute(tableType, typeof(TableAttribute)))?.Name;

            if (tableName == null)
            {
                tableName = this.dbSetProperties[tableType].Name;
            }

            return tableName;
        }

        private string[] GetEntityColumnNames(Type table)
        {
            string tableName = this.GetTableName(table);

            IEnumerable<string> dbColumns = this.connection.FetchColumnNames(tableName);

            string[] columns = table.GetProperties()
                .Where(pi => dbColumns.Contains(pi.Name) && !pi.HasAttribute<NotMappedAttribute>() && AllowedSqlTypes.Contains(pi.PropertyType))
                .Select(pi => pi.Name)
                .ToArray();

            return columns;
        }
    }
}