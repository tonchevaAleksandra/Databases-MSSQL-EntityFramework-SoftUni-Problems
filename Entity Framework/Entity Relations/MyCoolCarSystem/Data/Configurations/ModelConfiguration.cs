using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCoolCarSystem.Data.Models;

namespace MyCoolCarSystem.Data.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> model)
        {
            model
               .HasOne(m => m.Make)
               .WithMany(ma => ma.Models)
               .HasForeignKey(m => m.MakeId);
        }
    }
}
