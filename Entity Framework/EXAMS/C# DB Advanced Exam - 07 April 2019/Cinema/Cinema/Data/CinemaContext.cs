using Cinema.Data.Models;
using Cinema.Data.Models.Enums;

namespace Cinema.Data
{
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Seat> Seats { get; set; }

        public CinemaContext()  { }

        public CinemaContext(DbContextOptions options)
            : base(options)   { }

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
            modelBuilder.Entity<Projection>(p =>
            {
                p.HasOne(x => x.Movie)
                    .WithMany(y => y.Projections)
                    .HasForeignKey(x => x.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(x => x.Hall)
                    .WithMany(y => y.Projections)
                    .HasForeignKey(x => x.HallId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Ticket>(t =>
            {
                t.HasOne(x => x.Customer)
                    .WithMany(y => y.Tickets)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.Projection)
                    .WithMany(y => y.Tickets)
                    .HasForeignKey(x => x.ProjectionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Seat>(s =>
            {
                s.HasOne(x => x.Hall)
                    .WithMany(y => y.Seats)
                    .HasForeignKey(x => x.HallId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}