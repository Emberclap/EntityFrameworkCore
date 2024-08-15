using MiniORM.app.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM.app.Data
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext()
            : base() 
        {
            
        }
        public DbSet<Employee> Employees { get; }
        public DbSet<Project> Projects { get; }
        public DbSet<Department> Departments { get; }
        public DbSet<EmployeesProject> EmployeesProjects { get;}
    }
}
