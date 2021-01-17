using Microsoft.EntityFrameworkCore;


namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {

        public FootballBettingContext()
        {

        }
        //TODO: COMPLETE DbContext
        public FootballBettingContext( DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionConfiguration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }


}
