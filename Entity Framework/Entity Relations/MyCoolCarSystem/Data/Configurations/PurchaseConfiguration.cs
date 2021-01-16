using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCoolCarSystem.Data.Models;

namespace MyCoolCarSystem.Data.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<CarPurchase>
    {
        public void Configure(EntityTypeBuilder<CarPurchase> purchase)
        {
            purchase
               .HasKey(p => new { p.CarId, p.CustomerId });

            //entity.Property(p => p.PurchaseDate)
            //.ValueGeneratedOnAddOrUpdate();

            purchase
               .HasOne(p => p.Customer)
               .WithMany(c => c.Purchases)
               .HasForeignKey(p => p.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

            purchase
               .HasOne(p => p.Car)
               .WithMany(c => c.Owners)
               .HasForeignKey(p => p.CarId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
