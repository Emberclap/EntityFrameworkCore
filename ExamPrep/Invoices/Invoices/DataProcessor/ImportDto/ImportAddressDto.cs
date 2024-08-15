using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Data.DataConstraints;


namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Address))]
    public class ImportAddressDto
    {
        [Required]
        [XmlElement(nameof(StreetName))]
        [MinLength(AddressStreetNameMinValue)]
        [MaxLength(AddressStreetNameMaxValue)]
        public string StreetName { get; set; } = null!;
        [XmlElement(nameof(StreetNumber))]
        [Required]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement(nameof(PostCode))]
        public string PostCode { get; set; } = null!;

        [Required]
        [XmlElement(nameof(City))]
        [MinLength(AddressCityMinValue)]
        [MaxLength(AddressCityMaxValue)]
        public string City { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Country))]
        [MinLength(AddressCountryMinValue)]
        [MaxLength(AddressCountryMaxValue)]
        public string Country { get; set; } = null!;
    }
}
