namespace BookShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BookShopContext : DbContext
    {

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
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

            modelBuilder.Entity<AuthorBook>(entity =>
            {
                entity.HasKey(x => new { x.AuthorId, x.BookId });

                entity.HasOne(x => x.Author).WithMany(a => a.AuthorsBooks).HasForeignKey(x => x.AuthorId);

                entity.HasOne(x => x.Book).WithMany(b => b.AuthorsBooks).HasForeignKey(x => x.BookId);
            });


        }
    }
}