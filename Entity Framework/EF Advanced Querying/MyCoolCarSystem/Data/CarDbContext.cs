using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using MyCoolCarSystem.Data.Models;

namespace MyCoolCarSystem.Data
{
    public class CarDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CarPurchase> CarsOwners { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public override int SaveChanges()
        {
            var entries = this.ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                var validationContext = new ValidationContext(entity);

                Validator.ValidateObject(entity, validationContext, true);

            }

            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataConfiguration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Instead of => 

            //modelBuilder.ApplyConfiguration(new ModelConfiguration());

            //modelBuilder.ApplyConfiguration(new MakeConfiguration());

            //modelBuilder.ApplyConfiguration(new CarConfiguration());

            //modelBuilder.ApplyConfiguration(new PurchaseConfiguration());

            //modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
