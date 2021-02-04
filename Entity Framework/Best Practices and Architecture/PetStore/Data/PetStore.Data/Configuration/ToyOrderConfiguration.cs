using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
   public class ToyOrderConfiguration:IEntityTypeConfiguration<ToyOrder>
    {
        public void Configure(EntityTypeBuilder<ToyOrder> toyOrder)
        {
            toyOrder.HasKey(to => new {to.OrderId, to.ToyId});

            toyOrder.HasOne(to => to.Order)
                .WithMany(o => o.Toys)
                .HasForeignKey(to => to.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            toyOrder.HasOne(to => to.Toy)
                .WithMany(t => t.Orders)
                .HasForeignKey(to => to.ToyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
