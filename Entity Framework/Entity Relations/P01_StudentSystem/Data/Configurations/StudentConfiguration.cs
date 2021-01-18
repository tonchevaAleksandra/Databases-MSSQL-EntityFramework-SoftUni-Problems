using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> student)
        {
            student.HasKey(s => s.StudentId);

            student.Property(s => s.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(100);

            student.Property(s=>s.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasColumnType("CHAR(10)");

            student.Property(s => s.RegisteredOn)
                .IsRequired(true);

            student.Property(s => s.Birthday)
                .IsRequired(false);
                
        }
    }
}
