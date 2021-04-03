using Microsoft.EntityFrameworkCore;
using Stations.Models;

namespace Stations.Data
{
    public class StationsDbContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }

        public DbSet<TrainSeat> TrainSeats { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<SeatingClass> SeatingClasses { get; set; }

        public DbSet<CustomerCard> CustomerCards { get; set; }

        public StationsDbContext()
        {
        }

        public StationsDbContext(DbContextOptions options)
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
            builder.Entity<Station>(s =>
            {
                s.HasIndex(x => x.Name)
                    .IsUnique();
            });

            builder.Entity<Train>(t =>
            {
                t.HasIndex(x => x.TrainNumber)
                    .IsUnique();
            });

            builder.Entity<SeatingClass>(st =>
            {
                st.HasIndex(x => x.Name)
                    .IsUnique();

                st.HasIndex(x => x.Abbreviation)
                    .IsUnique();
            });

            builder.Entity<TrainSeat>(ts =>
            {
                ts.HasOne(x => x.Train)
                    .WithMany(y => y.TrainSeats)
                    .HasForeignKey(x => x.TrainId)
                    .OnDelete(DeleteBehavior.Restrict);

                ts.HasOne(x => x.SeatingClass);
            });

            builder.Entity<Trip>(t =>
            {
                t.HasOne(x => x.OriginStation)
                    .WithMany(y => y.TripsFrom)
                    .HasForeignKey(x => x.OriginStationId)
                    .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.DestinationStation)
                    .WithMany(y => y.TripsTo)
                    .HasForeignKey(x => x.DestinationStationId)
                    .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.Train)
                    .WithMany(y => y.Trips)
                    .HasForeignKey(x => x.TrainId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

        }
    }
}