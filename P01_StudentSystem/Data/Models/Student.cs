﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; } = null!;

        [MaxLength(10)]
        [MinLength(10)]
        public string? PhoneNumber { get; set; }
      
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday  { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }

    }
}
