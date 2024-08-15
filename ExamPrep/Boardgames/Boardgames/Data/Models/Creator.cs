using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstrains;
namespace Boardgames.Data.Models
{
    public class Creator
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(CreatorFirstNameMaxValue)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(CreatorLastNameMaxValue)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Boardgame> Boardgames { get; set; } = new List<Boardgame>();
    }
}