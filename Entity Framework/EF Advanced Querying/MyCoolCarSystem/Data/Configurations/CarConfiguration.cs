using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCoolCarSystem.Data.Models;

namespace MyCoolCarSystem.Data.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> car)
        {
            car
               .HasIndex(c => c.Vin)
               .IsUnique(true);

            car
               .HasOne(c => c.Model)
               .WithMany(m => m.Cars)
               .HasForeignKey(c => c.ModelId)
               .OnDelete(DeleteBehavior.Restrict);

            //car.Property<int>("MySecretProperty");

        }
    }
}
