using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Invoices.Data.Models;
using Invoices.Data.Models.Enums;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType(nameof(Client))]
    public class ExportClientsDto
    {
        [XmlAttribute(nameof(InvoicesCount))]
        public int InvoicesCount {  get; set; }
        [XmlElement(nameof(ClientName))]
        public string ClientName { get; set; } = null!;
        [XmlElement(nameof(VatNumber))]
        public string VatNumber { get; set; } = null!;
        [XmlArray(nameof(Invoices))]
        public ExportInvoicesDto[] Invoices {  get; set; }

    }


}
