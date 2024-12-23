﻿namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class TeisterMaskContext : DbContext
    {
        public TeisterMaskContext() 
        {
        }

        public TeisterMaskContext(DbContextOptions options)
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

            modelBuilder.Entity<EmployeeTask>()
                .HasKey(e => new {e.EmployeeId, e.TaskId});
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<EmployeeTask> EmployeesTasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
    }
}