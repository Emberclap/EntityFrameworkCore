using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Data.DataConstrains;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(District))]
    public class ImportDistrictsDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(DistrictNameMinValue)]
        [MaxLength(DistrictNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement(nameof(PostalCode))]
        [RegularExpression(@"^[A-Z]{2}-[0-9]{5}$")]
        public string PostalCode { get; set; } = null!;

        [XmlArray(nameof(Properties))]
        [Required]
        public ImportPropertyDto[] Properties { get; set; } = null!;

        [Required]
        [XmlAttribute(nameof(Region))]
        public string Region { get; set; } = null!;
    }
}
