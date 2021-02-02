using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configuration
{
   public class ToyConfiguration:IEntityTypeConfiguration<Toy>
    {
        public void Configure(EntityTypeBuilder<Toy> toy)
        {
            toy.HasOne(t => t.Brand)
                .WithMany(b => b.Toys)
                .HasForeignKey(t => t.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            toy.HasOne(t => t.Category)
                .WithMany(c => c.Toys)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
