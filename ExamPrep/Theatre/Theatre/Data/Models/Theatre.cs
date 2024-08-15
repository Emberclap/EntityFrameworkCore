using System.ComponentModel.DataAnnotations;
using static Theatre.Data.DataConstraints;
namespace Theatre.Data.Models
{
    public class Theatre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(TheatreNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(TheatreDirectorMaxValue)]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
