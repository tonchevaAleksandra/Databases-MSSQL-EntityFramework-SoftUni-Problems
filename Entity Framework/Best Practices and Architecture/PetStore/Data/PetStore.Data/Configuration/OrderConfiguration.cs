using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
   public class OrderConfiguration :IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> order)
        {
            order.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
