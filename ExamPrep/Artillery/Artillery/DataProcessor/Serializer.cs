
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                 .Where(s => s.ShellWeight >= shellWeight)
                 .Select(s => new
                 {
                     s.ShellWeight,
                     s.Caliber,
                     Guns = s.Guns
                     .Where(g => (int)g.GunType == 3)
                     .Select(g => new
                     {
                         GunType = g.GunType.ToString(),
                         g.GunWeight,
                         g.BarrelLength,
                         Range = g.Range >= 3000 ? "Long-range" : "Regular range"
                     })
                     .OrderByDescending(g => g.GunWeight)
                     .ToList()
                 })
                 .OrderBy(s => s.ShellWeight)
                 .ToList();

            return JsonConvert.SerializeObject(shells , Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
                .Where(m => m.Manufacturer.ManufacturerName == manufacturer)
                .Select(m => new ExportGunsDto
                {
                    Manufacturer = m.Manufacturer.ManufacturerName,
                    GunType = m.GunType,
                    GunWeight = m.GunWeight,
                    BarrelLength = m.BarrelLength,
                    Range = m.Range,
                    Countries = m.CountriesGuns
                        .Where(m => m.Country.ArmySize > 4_500_000)
                        .Select(c => new ExportCountriesDto
                        {
                            Country = c.Country.CountryName,
                            ArmySize = c.Country.ArmySize
                        })
                        .OrderBy(c => c.ArmySize)
                        .ToArray()
                })
                .OrderBy(b => b.BarrelLength)
                .ToList();

            return XmlSerializationHelper.Serialize(guns, "Guns");
        }
    }
}
