namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
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
                optionsBuilder.UseSqlServer("Server=WDESK\\SQLEXPRESS;Database=MusicHub;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SongPerformer>()
                .HasKey(sc => new { sc.SongId, sc.PerformerId });

        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}

