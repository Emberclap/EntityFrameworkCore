using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Invoices.Data
{
    public class InvoicesContext : DbContext
    {
        public InvoicesContext() 
        { 
        }

        public InvoicesContext(DbContextOptions options)
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
            modelBuilder.Entity<ProductClient>()
                .HasKey(pc => new { pc.ProductId, pc.ClientId});
        }

        public virtual DbSet<ProductClient> ProductsClients { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Product> Products { get; set; }

    }
}
