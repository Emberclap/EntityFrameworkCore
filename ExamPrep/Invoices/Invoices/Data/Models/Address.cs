using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(AddressStreetNameMaxValue)]
        public string StreetName { get; set; } = null!;
        public int StreetNumber { get; set; }
        [Required]
        public string PostCode { get; set; } = null!;
        [Required]
        [MaxLength(AddressCityMaxValue)]
        public string City { get; set; } = null!;
        [Required]
        [MaxLength(AddressCountryMaxValue)]
        public string Country { get; set; } = null!;
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }
    }
}
