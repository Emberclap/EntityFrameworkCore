using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TravelAgency.Data.Models;
using static TravelAgency.Data.DataConstraints;
namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType(nameof(Customer))]
    public class ImportCustomerDto
    {
        [Required]
        [XmlAttribute(nameof(phoneNumber))]
        [RegularExpression(CustomerPhoneNumberRegex)]
        public string phoneNumber { get; set; } = null!;
        [Required]
        [XmlElement(nameof(FullName))]
        [MinLength(CustomerFullNameMinValue), MaxLength(CustomerFullNameMaxValue)]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Email))]
        [MinLength(CustomerEmailMinValue), MaxLength(CustomerEmailMaxValue)]
        public string Email { get; set; } = null!;
    }
}
