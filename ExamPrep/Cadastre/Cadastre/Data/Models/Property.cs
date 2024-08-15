using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cadastre.Data.DataConstrains;
namespace Cadastre.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(PropertyMaxIdentifierValue)]
        public string PropertyIdentifier { get; set; } = null!;
        public int Area { get; set; }
        [MaxLength(PropertyDetailsMaxValue)]
        public string? Details { get; set; }
        [Required, MaxLength(PropertyAddressMaxValue)]
        public string Address { get; set; } = null!;
        public DateTime DateOfAcquisition { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; } = null!;

        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();

    }
}
