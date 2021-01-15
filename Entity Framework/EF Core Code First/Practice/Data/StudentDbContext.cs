using Microsoft.EntityFrameworkCore;
using Practice.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Practice.Data
{
    public class StudentDbContext : DbContext
    {

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<StudentInCourse> StudentsCourses { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataSettings.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.ToTable("Peshos");
            //    entity.Property(e => e.FirstName)
            //    .IsRequired()
            //    .HasMaxLength(30);
            //});

            modelBuilder
                .Entity<Student>()
                .HasOne(st => st.Town)
                .WithMany(testc => testc.Students)
                .HasForeignKey(st => st.TownId);

            //modelBuilder.Entity<Town>()
            //    .HasMany(t => t.Students)
            //    .WithOne(st => st.Town)
            //    .HasForeignKey(st => st.TownId);

            modelBuilder
                .Entity<Student>()
                .HasMany(st => st.Homeworks)
                .WithOne(h => h.Student)
                .HasForeignKey(h => h.StudentId);

            modelBuilder
                .Entity<Course>()
                .HasMany(c => c.Homeworks)
                .WithOne(h => h.Course)
                .HasForeignKey(H => H.CourseId);

            modelBuilder
                .Entity<StudentInCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder
                .Entity<StudentInCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(st => st.Courses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder
                .Entity<StudentInCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CourseId);


            //modelBuilder.Entity<Town>()
            //    .HasData(new
            //    { Name = "Plovdiv" });
        }
    }
}
