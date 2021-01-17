using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.HasKey(u => u.UserId);

            user.Property(u => u.Username)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(50);

            user.Property(u => u.Password)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(256);

            user.Property(u => u.Email)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(50);

            user.Property(u => u.Name)
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(100);
                
        }
    }
}
