using Artillery.Data.Models;
using static Artillery.Data.DataConstrains;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(nameof(Country))]
    public class ImportCountriesDto
    {
        [Required]
        [XmlElement(nameof(CountryName))]
        [MinLength(CountryCountryNameMinValue)]
        [MaxLength(CountryCountryNameMaxValue)]
        public string CountryName { get; set; } = null!;

        [XmlElement(nameof(ArmySize))]
        [Range(CountryArmySizeMinValue, CountryArmySizeMaxValue)]
        public int ArmySize { get; set; }
    }
}
