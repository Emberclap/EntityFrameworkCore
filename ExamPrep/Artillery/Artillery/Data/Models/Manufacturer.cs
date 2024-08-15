using System.ComponentModel.DataAnnotations;
using static Artillery.Data.DataConstrains;
namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ManufacturerNameMaxValue)]
        public string ManufacturerName { get; set; } = null!;
        [Required]
        [MaxLength(ManufacturerFoundedMaxValue)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns  { get; set; } = new HashSet<Gun>();
    }
}
