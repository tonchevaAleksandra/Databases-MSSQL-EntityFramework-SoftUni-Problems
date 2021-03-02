using MusicHub.Data.Models;

namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;

    public class MusicHubDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
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
            builder.Entity<SongPerformer>(sp =>
            {
                sp.HasKey(x => new {x.PerformerId, x.SongId});
                sp.HasOne(x => x.Song)
                    .WithMany(y => y.SongPerformers)
                    .HasForeignKey(x => x.SongId)
                    .OnDelete(DeleteBehavior.Restrict);
                sp.HasOne(x => x.Performer)
                    .WithMany(y => y.PerformerSongs)
                    .HasForeignKey(x => x.PerformerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Song>(song =>
            {
                //song.HasOne(x => x.Writer)
                //    .WithMany(y => y.Songs)
                //    .HasForeignKey(x => x.WriterId)
                //    .OnDelete(DeleteBehavior.Restrict);

                song.HasOne(x => x.Album)
                    .WithMany(y => y.Songs)
                    .HasForeignKey(x => x.AlbumId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

        }
    }
}
