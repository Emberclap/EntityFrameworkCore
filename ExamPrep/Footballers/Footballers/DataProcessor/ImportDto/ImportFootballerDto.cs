using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Data.DataConstraints;
namespace Footballers.DataProcessor.ImportDto
{
    [XmlType(nameof(Footballer))]
    public class ImportFootballerDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(FootballerNameMinValue)]
        [MaxLength(FootballerNameMaxValue)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(ContractStartDate))]
        public string ContractStartDate { get; set; } = null!;

        [XmlElement(nameof(ContractEndDate))]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement(nameof(BestSkillType))]
        [Range(FootballerBestSkillTypeMinValue, FootballerBestSkillTypeMaxValue)]
        public int BestSkillType { get; set; }

        [XmlElement(nameof(PositionType))]
        [Range(FootballerPositionTypeMinValue, FootballerPositionTypeMaxValue)]
        public int PositionType { get; set; }
    }
}