﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoftUni.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Departments = new HashSet<Department>();
            InverseManager = new HashSet<Employee>();
            EmployeesProjects = new HashSet<EmployeeProjects>();
        }

        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string? MiddleName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string JobTitle { get; set; } = null!;
        [Column("DepartmentID")]
        public int DepartmentId { get; set; }
        [Column("ManagerID")]
        public int? ManagerId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime HireDate { get; set; }
        [Column(TypeName = "decimal(15, 4)")]
        public decimal Salary { get; set; }
        [Column("AddressID")]
        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("Employees")]
        public virtual Address? Address { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("Employees")]
        public virtual Department Department { get; set; } = null!;
        [ForeignKey("ManagerId")]
        [InverseProperty("InverseManager")]
        public virtual Employee? Manager { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<Department> Departments { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<Employee> InverseManager { get; set; }

        public virtual ICollection<EmployeeProjects> EmployeesProjects { get; set; }
    }
}
