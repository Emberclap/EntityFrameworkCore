using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(InvoiceNumberMinRange, InvoiceNumberMaxRange)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; } = null!;
        [Required]
        public string DueDate { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Range(InvoiceCurrencyTypeMinValue, InvoiceCurrencyTypeMaxValue)]
        public int CurrencyType { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
