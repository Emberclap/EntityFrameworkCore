namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.Data.SqlClient.Server;
    using System.Globalization;
    using System.Security.Principal;
    using System.Text.Json;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .Select(c => new ExportClientsDto ()
                {
                    InvoicesCount = c.Invoices.Count,
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(i => new ExportInvoicesDto ()
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        Currency = i.CurrencyType
                    })
                    .ToArray()
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToList();

            return XmlSerializationHelper.Serialize(clients, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context.Products
                .Where(p => p.ProductsClients.Any(p => p.Client.Name.Length >= nameLength))
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                    .Where(p => p.Client.Name.Length >= nameLength)
                    .Select(p => new
                    {
                        Name = p.Client.Name,
                        NumberVat = p.Client.NumberVat
                    })
                    .OrderBy(p => p.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.Clients.Count())
                .ThenBy(p => p.Name)
                .Take(5)
                .ToList();

            return JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true } );
        }
    }
}