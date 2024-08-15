using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Data.DataConstrains;
namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(Property))]
    public class ImportPropertyDto
    {
        [Required]
        [XmlElement(nameof(PropertyIdentifier))]
        [MinLength(PropertyMinIdentifierValue)]
        [MaxLength(PropertyMaxIdentifierValue)]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement(nameof(Area))]
        [Range(0,int.MaxValue)]
        public int Area { get; set; }

        [XmlElement(nameof(Details))]
        [MinLength(PropertyDetailsMinValue)]
        [MaxLength(PropertyDetailsMaxValue)]
        public string? Details { get; set; }
        [Required]
        [XmlElement(nameof(Address))]
        [MinLength(PropertyAddressMinValue)]
        [MaxLength(PropertyAddressMaxValue)]
        public string Address { get; set; } = null!;
        [Required]
        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; } = null!;
       
    }
}
