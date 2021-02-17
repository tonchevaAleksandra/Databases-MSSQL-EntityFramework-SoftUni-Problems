using VaporStore.Data.Models;

namespace VaporStore.Data
{
    using Microsoft.EntityFrameworkCore;

    public class VaporStoreDbContext : DbContext
    {

        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public VaporStoreDbContext()
        {
        }

        public VaporStoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Game>(game =>
            {
                game.HasOne(e => e.Developer)
                    .WithMany(d => d.Games)
                    .HasForeignKey(e => e.DeveloperId)
                    .OnDelete(DeleteBehavior.Restrict);

                game.HasOne(e => e.Genre)
                    .WithMany(g => g.Games)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            model.Entity<GameTag>(gameTag =>
            {
                gameTag.HasKey(gt => new { gt.GameId, gt.TagId });

                gameTag.HasOne(gt => gt.Game)
                    .WithMany(g => g.GameTags)
                    .HasForeignKey(gt => gt.GameId)
                    .OnDelete(DeleteBehavior.Restrict);

                gameTag.HasOne(gt => gt.Tag)
                    .WithMany(t => t.GameTags)
                    .HasForeignKey(gt => gt.TagId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            model.Entity<Card>(card =>
            {
                card.HasOne(c => c.User)
                    .WithMany(u => u.Cards)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            model.Entity<Purchase>(purchase =>
            {
                purchase.HasOne(p => p.Game)
                    .WithMany(g => g.Purchases)
                    .HasForeignKey(p => p.GameId)
                    .OnDelete(DeleteBehavior.Restrict);

                purchase.HasOne(p => p.Card)
                    .WithMany(c => c.Purchases)
                    .HasForeignKey(p => p.CardId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}