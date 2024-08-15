using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Artillery.Data.DataConstrains;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(nameof(Manufacturer))]
    public class ImportManufacturersDto
    {
        [Required]
        [XmlElement(nameof(ManufacturerName))]
        [MinLength(ManufacturerNameMinValue)]
        [MaxLength(ManufacturerNameMaxValue)]
        public string ManufacturerName { get; set; } = null!;
        [Required]
        [XmlElement(nameof(Founded))]
        [MinLength(ManufacturerFoundedMinValue)]
        [MaxLength(ManufacturerFoundedMaxValue)]
        public string Founded { get; set; } = null!;
    }
}
