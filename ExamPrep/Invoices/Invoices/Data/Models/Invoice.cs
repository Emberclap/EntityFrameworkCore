using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invoices.Data.Models.Enums;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        [Range(InvoiceNumberMinRange, InvoiceNumberMaxRange)]
        public int Number { get; set; }
        public DateTime IssueDate { get; set; } 
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;
    }
}
