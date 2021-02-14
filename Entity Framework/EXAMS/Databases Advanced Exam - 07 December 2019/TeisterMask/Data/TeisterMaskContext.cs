namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;

    using TeisterMask.Data.Models;
    public class TeisterMaskContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeTask> EmployeesTasks { get; set; }
        public TeisterMaskContext() { }

        public TeisterMaskContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Task>().HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeTask>(entity =>
            {
                entity.HasKey(et => new
                    {et.EmployeeId, et.TaskId});

                entity.HasOne(e => e.Employee)
                    .WithMany(em => em.EmployeesTasks)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Task)
                    .WithMany(t => t.EmployeesTasks)
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}