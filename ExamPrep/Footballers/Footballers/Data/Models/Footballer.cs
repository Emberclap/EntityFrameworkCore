using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footballers.Data.Models.Enums;
using static Footballers.Data.DataConstraints;
namespace Footballers.Data.Models
{
    public class Footballer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(FootballerNameMaxValue)]
        public string Name { get; set; } = null!;
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public PositionType PositionType { get; set; }
        public BestSkillType BestSkillType { get; set; }
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }
        [Required]
        public Coach Coach { get; set; } = null!;

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();
    }
}
