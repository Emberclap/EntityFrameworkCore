using Footballers.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Data.DataConstraints;
namespace Footballers.DataProcessor.ImportDto
{
    [XmlType(nameof(Coach))]
    public class ImportCoachesDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(CoachNameMinValue)]
        [MaxLength(CoachNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray(nameof(Footballers))]
        public ImportFootballerDto[] Footballers { get; set; } = null!;
    }
}
