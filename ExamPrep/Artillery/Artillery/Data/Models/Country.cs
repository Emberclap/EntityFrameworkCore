using System.ComponentModel.DataAnnotations;
using static Artillery.Data.DataConstrains;
namespace Artillery.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(CountryCountryNameMaxValue)]
        public string CountryName { get; set; } = null!;
        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get;} = new HashSet<CountryGun>();

    }
}
