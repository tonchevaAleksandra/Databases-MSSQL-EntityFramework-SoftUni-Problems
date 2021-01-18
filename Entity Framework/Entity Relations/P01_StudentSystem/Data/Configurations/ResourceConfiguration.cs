using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> resource)
        {
            resource.HasKey(r => r.ResourceId);

            resource.Property(r => r.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);

            resource.Property(r => r.Url)
                .IsRequired(true)
                .IsUnicode(false);

            resource
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
