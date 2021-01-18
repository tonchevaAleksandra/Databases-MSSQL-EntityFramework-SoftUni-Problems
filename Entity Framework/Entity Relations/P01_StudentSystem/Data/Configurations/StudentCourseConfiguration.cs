using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> studentCourse)
        {
            studentCourse.HasKey(st => new { st.StudentId, st.CourseId });

            studentCourse
                .HasOne(st => st.Course)
                .WithMany(c => c.StudentsEnrolled)
                .HasForeignKey(st => st.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            studentCourse
                .HasOne(st => st.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
