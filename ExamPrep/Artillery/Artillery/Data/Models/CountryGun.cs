using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class CountryGun
    {
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        [Required]
        public Country Country { get; set; } = null!;
        [ForeignKey(nameof(Gun))]
        public int GunId { get; set; }
        [Required]
        public Gun Gun { get; set; } = null!;
    }
}