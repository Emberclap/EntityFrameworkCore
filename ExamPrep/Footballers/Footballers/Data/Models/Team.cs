using System.ComponentModel.DataAnnotations;
using static Footballers.Data.DataConstraints;
namespace Footballers.Data.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(TeamNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(TeamNationalityMaxValue)]
        public string Nationality { get; set; } = null!;

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();
    }
}
