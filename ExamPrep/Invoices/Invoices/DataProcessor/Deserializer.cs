namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Microsoft.VisualBasic;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var clientsToImport = new List<Client>();

            ImportClientDto[] deserializedClients = XmlSerializationHelper
                .Deserialize<ImportClientDto[]>(xmlString, "Clients");

            foreach (var clientDto in deserializedClients)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var addressesToImport = new List<Address>();

                foreach (var addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newAddress = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country,
                    };
                    addressesToImport.Add(newAddress);
                }
                
                var client = new Client ()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = addressesToImport
                };
                clientsToImport.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, clientDto.Name));
            }
            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
           var sb = new StringBuilder();

            ImportInvoiceDto[] invoicesDtos = JsonSerializer
                .Deserialize<ImportInvoiceDto[]>(jsonString);

            var invoicesToImport = new List<Invoice>();

            foreach (var invoiceDto in invoicesDtos)
            {
                bool isIssueDateValid = DateTime
                 .TryParse(invoiceDto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);
                bool isDueDateValid = DateTime
                    .TryParse(invoiceDto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (!IsValid(invoiceDto) 
                    || isIssueDateValid == false 
                    || isDueDateValid == false 
                    || DateTime.Compare(dueDate, issueDate) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
             
                if (!context.Clients.Any(cl => cl.Id == invoiceDto.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };
                invoicesToImport.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoiceDto.Number));
            }
            context.Invoices.AddRange(invoicesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var productDtos = JsonSerializer
                .Deserialize<ImportProductDto[]>(jsonString);

            var productsToImport = new List<Product>();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                }

                var newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType,
                };

                ICollection<ProductClient> productClientsToImport = new List<ProductClient>();
                foreach (var clientId in productDto.Clients.Distinct())
                {
                    if (!context.Clients.Any(cl => cl.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var newProductClient = new ProductClient()
                    {
                        Product = newProduct,
                        ClientId = clientId,
                    };
                    productClientsToImport.Add(newProductClient);
                }
                newProduct.ProductsClients = productClientsToImport;

                productsToImport.Add(newProduct);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, productDto.Name, productClientsToImport.Count));
            }

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true );
        }
    } 
}
