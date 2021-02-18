using SoftJail.Data.Models;

namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;

	public class SoftJailDbContext : DbContext
	{

        public DbSet<Prisoner> Prisoners { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Prisoner>(prisoner =>
            //{
            //    prisoner.HasOne(p => p.Cell)
            //        .WithMany(c => c.Prisoners)
            //        .HasForeignKey(p => p.CellId)
            //        .OnDelete(DeleteBehavior.Restrict);

            //});

            //builder.Entity<Cell>(cell =>
            //{
            //    cell.HasOne(c => c.Department)
            //        .WithMany(d => d.Cells)
            //        .HasForeignKey(c => c.DepartmentId)
            //        .OnDelete(DeleteBehavior.Restrict);
            //});

            //builder.Entity<Mail>(mail =>
            //{
            //    mail.HasOne(m => m.Prisoner)
            //        .WithMany(p => p.Mails)
            //        .HasForeignKey(m => m.PrisonerId)
            //        .OnDelete(DeleteBehavior.Restrict);
            //});

            builder.Entity<OfficerPrisoner>(offPr =>
            {
                offPr.HasKey(of => new {of.PrisonerId, of.OfficerId });

                offPr.HasOne(of => of.Prisoner)
                    .WithMany(p => p.PrisonerOfficers)
                    .HasForeignKey(of => of.PrisonerId)
                    .OnDelete(DeleteBehavior.Restrict);

                offPr.HasOne(of => of.Officer)
                    .WithMany(o => o.OfficerPrisoners)
                    .HasForeignKey(of => of.OfficerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
	}
}