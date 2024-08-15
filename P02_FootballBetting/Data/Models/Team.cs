using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {

        [Key]
        public int TeamId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = null!;

        public string LogoUrl { get; set; }
        [MaxLength(3)]
        public string Initials { get; set; }
        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set;}
        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }


        public virtual ICollection<Game> HomeGames { get; set; }
        public virtual ICollection<Game> AwayGames { get; set; }
        public virtual ICollection<Player> Players { get; set; }

    }
}
