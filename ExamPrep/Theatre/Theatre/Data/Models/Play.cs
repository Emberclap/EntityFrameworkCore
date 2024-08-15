using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Theatre.Data.Models.Enums;
using static Theatre.Data.DataConstraints;
namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(PlayTitleMaxValue)]
        public string Title { get; set; } = null!;
        public TimeSpan Duration { get; set; }
        public float Rating { get; set; }
        public Genre Genre { get; set; } 
        [Required]
        [MaxLength(PlayDescriptionMaxValue)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(PlayScreenwriterMaxValue)]
        public string Screenwriter { get; set; } = null!;
        public virtual ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
