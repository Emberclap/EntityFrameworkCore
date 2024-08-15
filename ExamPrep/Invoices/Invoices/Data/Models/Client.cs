using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();
    }
}