using Invoices.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            var sb = new StringBuilder();
            const string xmlRoot = "Customers";

            var customerDtos = XmlSerializationHelper
                .Deserialize<ImportCustomerDto[]>(xmlString, xmlRoot);

            var customersToImport = new List<Customer>();

            foreach (var c in customerDtos )
            {
                if(!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(customersToImport.Any(u => u.FullName == c.FullName) 
                    || customersToImport.Any(u => u.Email == c.Email 
                    || customersToImport.Any(u => u.PhoneNumber == c.phoneNumber)))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }
                var newCustomer = new Customer 
                {
                    PhoneNumber = c.phoneNumber,
                    FullName = c.FullName,
                    Email = c.Email,
                };
                customersToImport.Add(newCustomer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
            }
            context.Customers.AddRange(customersToImport);
            context.SaveChanges();
    
            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var bookingsDtos = JsonConvert
                .DeserializeObject<ImportBookingsDto[]>(jsonString);

            var bookingsToImport = new List<Booking>();
            foreach(var b in bookingsDtos)
            {
                bool isBookingDateValid = DateTime
                    .TryParseExact(b.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);
               
                if(!IsValid(b) || !isBookingDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var customer = context.Customers
                    .First(c => c.FullName == b.CustomerName);
                var tourPackage = context.TourPackages
                    .First(t => t.PackageName == b.TourPackageName);


                var newBooking = new Booking
                {
                    BookingDate = bookingDate,
                    Customer = customer,
                    TourPackage = tourPackage
                };
                bookingsToImport.Add(newBooking);
                sb.AppendLine(string
                    .Format(SuccessfullyImportedBooking , newBooking.TourPackage.PackageName , bookingDate.ToString("yyyy-MM-dd")));
            }
            context.Bookings.AddRange(bookingsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
