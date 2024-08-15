using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Client))]
    public class ImportClientDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(ClientNameMinLength)]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;


        [Required]
        [XmlElement(nameof(NumberVat))]
        [MinLength(ClientNumberVatMinLength)]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray(nameof(Addresses))]
        public ImportAddressDto[] Addresses { get; set; } = null!;
    }
}
