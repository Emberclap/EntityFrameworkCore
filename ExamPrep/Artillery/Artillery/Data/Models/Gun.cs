using Artillery.Data.Models.Enums;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Gun
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }
        [Required]
        public Manufacturer Manufacturer { get; set; } = null!;

        public int GunWeight { get; set; }
        public double BarrelLength { get; set; }
        public int? NumberBuild { get; set; }

        public int Range { get; set; }
        public GunType GunType { get; set; }
        public int ShellId { get; set; }
        [Required]
        public Shell Shell { get; set; } = null!;

        public virtual ICollection<CountryGun> CountriesGuns { get; set;} =new HashSet<CountryGun>();
    }
}