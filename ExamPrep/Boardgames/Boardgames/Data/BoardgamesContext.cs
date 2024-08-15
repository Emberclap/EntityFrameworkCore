namespace Boardgames.Data
{
    using Boardgames.Data.Models;
    using Microsoft.EntityFrameworkCore;
    
    public class BoardgamesContext : DbContext
    {
        public BoardgamesContext()
        { 
        }

        public BoardgamesContext(DbContextOptions options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoardgameSeller>()
                .HasKey(e => new { e.BoardgameId, e.SellerId });
        }

        public virtual DbSet<Creator> Creators { get; set; }
        public virtual DbSet<Boardgame> Boardgames { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; } 
        public virtual DbSet<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
