﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }

        public sbyte RowNumber { get; set; }

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }
        [Required]
        public Play Play { get; set; } = null!;
        [ForeignKey(nameof(Theatre))]
        public int TheatreId { get; set; }
        [Required]
        public Theatre Theatre { get; set; } = null!;
    }
}