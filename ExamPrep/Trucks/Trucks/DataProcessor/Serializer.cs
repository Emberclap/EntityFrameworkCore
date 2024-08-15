namespace Trucks.DataProcessor
{
    using Data;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(de => de.Trucks.Any())
                .Select(d => new ExportDespatchersDto
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count(),
                    Trucks = d.Trucks
                    .Select(t => new ExportTrucksDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = (MakeType)t.MakeType,
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(de => de.TrucksCount)
                .ThenBy(de => de.DespatcherName)
                .ToList();

            return XmlSerializationHelper.Serialize(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .ToArray()
                .Where(c => c.ClientsTrucks
                .Any(t => t.Truck.TankCapacity >= capacity))
                    .Select(c => new
                    {
                        Name = c.Name,
                        Trucks = c.ClientsTrucks
                        .Select(t => t.Truck)
                        .Where(tc => tc.TankCapacity >= capacity)
                        .OrderBy(t => t.MakeType)
                        .ThenByDescending(t => t.CargoCapacity)
                            .Select(t => new
                            {
                                TruckRegistrationNumber = t.RegistrationNumber,
                                VinNumber = t.VinNumber,
                                TankCapacity = t.TankCapacity,
                                CargoCapacity = t.CargoCapacity,
                                CategoryType = t.CategoryType.ToString(),
                                MakeType = t.MakeType.ToString(),
                            })
                            .ToList()
                    })
                    .OrderByDescending(c => c.Trucks.Count)
                    .ThenBy(c => c.Name)
                    .Take(10)
                    .ToList();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);

        }
    }
}
