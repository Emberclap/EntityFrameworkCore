﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{

    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        public string? Url { get; set; }

        public ResourceType ResourceType { get; set; }

        public int CourseId {  get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; } = null!;

    }

    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other
    }
}
