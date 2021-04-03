using PetClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace PetClinic.Data
{

    public class PetClinicContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        public DbSet<AnimalAid> AnimalAids { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Vet> Vets { get; set; }

        public PetClinicContext()
        {
        }

        public PetClinicContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProcedureAnimalAid>(pa =>
            {
                pa.HasKey(x => new {x.ProcedureId, x.AnimalAidId});

                pa.HasOne(x => x.AnimalAid)
                    .WithMany(y => y.AnimalAidProcedures)
                    .HasForeignKey(x => x.AnimalAidId)
                    .OnDelete(DeleteBehavior.Restrict);

                pa.HasOne(x => x.Procedure)
                    .WithMany(y => y.ProcedureAnimalAids)
                    .HasForeignKey(x => x.ProcedureId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Procedure>(p =>
            {
                p.HasOne(x => x.Animal)
                    .WithMany(y => y.Procedures)
                    .HasForeignKey(x => x.AnimalId)
                    .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(x => x.Vet)
                    .WithMany(y => y.Procedures)
                    .HasForeignKey(x => x.VetId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}