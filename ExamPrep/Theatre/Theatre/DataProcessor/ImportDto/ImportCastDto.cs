using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models;
using static Theatre.Data.DataConstraints;
namespace Theatre.DataProcessor.ImportDto
{
    [XmlType(nameof(Cast))]
    public class ImportCastDto
    {
        [Required]
        [XmlElement(nameof(FullName))]
        [MinLength(CastFullNameMinValue), MaxLength(CastFullNameMaxValue)]
        public string FullName { get; set; } = null!;

        public bool IsMainCharacter { get; set; }
        [Required]
        [XmlElement(nameof(PhoneNumber))]
        [RegularExpression(CastPhoneNumberRegex)]
        public string PhoneNumber { get; set; } = null!;
        public int PlayId { get; set; }
    }
}
