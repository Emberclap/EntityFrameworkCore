﻿
namespace Theatre.Data
{
    using Microsoft.EntityFrameworkCore;
    using Theatre.Data.Models;

    public class TheatreContext : DbContext
    {
        public TheatreContext() 
        {
        }

        public TheatreContext(DbContextOptions options)
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

        public virtual DbSet<Cast> Casts { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Theatre> Theatres { get; set; }
        public virtual DbSet<Play> Plays { get; set; }
    }
}