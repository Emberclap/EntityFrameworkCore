﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; } = null!;
        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }
        [Required]
        public Task Task { get; set; } = null!;
    }
}