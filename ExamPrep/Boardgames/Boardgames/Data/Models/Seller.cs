using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstrains;
namespace Boardgames.Data.Models
{
    public class Seller
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(SellerNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(SellerAddressMaxValue)]
        public string Address { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string Website { get; set; } = null!;

        public virtual ICollection<BoardgameSeller>  BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();
    }
}
