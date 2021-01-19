using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCoolCarSystem.Data.Models;

namespace MyCoolCarSystem.Data.Configurations
{
    public class MakeConfiguration : IEntityTypeConfiguration<Make>
    {
        public void Configure(EntityTypeBuilder<Make> make)
        {
            make
               .HasIndex(m => m.Name)
               .IsUnique(true);
        }
    }
}
