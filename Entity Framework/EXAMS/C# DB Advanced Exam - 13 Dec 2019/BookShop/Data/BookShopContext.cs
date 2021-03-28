using BookShop.Data.Models;

namespace BookShop.Data
{
    using Microsoft.EntityFrameworkCore;

    public class BookShopContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<AuthorBook> AuthorsBooks { get; set; }

        public BookShopContext() { }

        public BookShopContext(DbContextOptions options)
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
            modelBuilder.Entity<AuthorBook>(ab =>
            {
                ab.HasKey(x => new {x.AuthorId, x.BookId});
                ab.HasOne(x => x.Author)
                    .WithMany(y => y.AuthorsBooks)
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                ab.HasOne(x => x.Book)
                    .WithMany(y => y.AuthorsBooks)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}