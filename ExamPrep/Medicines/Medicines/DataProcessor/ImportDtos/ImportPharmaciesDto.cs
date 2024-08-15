using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Data.DataConstraints;
namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Pharmacy))]
    public class ImportPharmaciesDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(PharmacyNameMinValue)]
        [MaxLength(PharmacyNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(PharmacyPhoneNumberRegex)]
        public string PhoneNumber { get; set; } = null!;
        [XmlArray(nameof(Medicines))]
        public ImportMedicineDto[] Medicines { get; set; } = null!;


    }
}
