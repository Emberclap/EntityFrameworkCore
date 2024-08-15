using Boardgames.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Boardgames.Data.DataConstrains;
namespace Boardgames.Data.Models
{
    public class Boardgame
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(BoardgameNameMaxValue)]
        public string Name { get; set; } = null!;
        public double Rating { get; set; }

        public int YearPublished { get; set; }

        public CategoryType CategoryType { get; set; }
        [Required]
        public string Mechanics { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public Creator Creator { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }  = new HashSet<BoardgameSeller>();
    }
}
