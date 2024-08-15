using Invoices.Utilities;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guidesWithSpanish = context.Guides
                .Where(g => (int)g.Language == 3)
                .OrderByDescending(g => g.TourPackagesGuides.Count())
                .ThenBy(g => g.FullName)
                .Select(g => new ExportGuideWithSpanish
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                    .OrderByDescending(p => p.TourPackage.Price)
                    .Select(t => new ExportTourPackageDto
                    {
                        Name = t.TourPackage.PackageName,
                        Description = t.TourPackage.Description,
                        Price = t.TourPackage.Price.ToString("f2"),
                    })
                    .ToArray()
                })
                .ToList();
            return XmlSerializationHelper.Serialize(guidesWithSpanish, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(c => c.Bookings.Any(t => t.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                    .Where(c => c.TourPackage.PackageName == "Horse Riding Tour")
                    .OrderBy(b =>b.BookingDate)
                    .Select(b => new
                    {
                        TourPackageName = b.TourPackage.PackageName,
                        Date = b.BookingDate.ToString("yyyy-MM-dd")
                    }).ToArray()
                })
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }
    }
}
