using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Data.DataConstrains;

namespace Cadastre.Data.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(DistrictNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        public Region Region { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    }
}