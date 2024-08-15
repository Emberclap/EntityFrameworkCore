namespace Cadastre.Data
{
    using Cadastre.Data.Models;
    using Microsoft.EntityFrameworkCore;
    public class CadastreContext : DbContext
    {
        public CadastreContext()
        {
            
        }

        public CadastreContext(DbContextOptions options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyCitizen>()
                .HasKey(p => new { p.PropertyId, p.CitizenId });
           
        }

        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyCitizen> PropertiesCitizens { get; set; }
        public virtual DbSet<District> Districts { get; set; }
    }
}
