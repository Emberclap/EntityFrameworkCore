namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Data;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var sb = new StringBuilder();
            const string xmlRoot = "Despatchers";

            var despatchersDtos = XmlSerializationHelper
                .Deserialize<ImportDespatchersDto[]>(xmlString, xmlRoot);
            var despatchersToImport = new List<Despatcher>();

            foreach(var de in despatchersDtos)
            {
                if (!IsValid(de) || de.Position == string.Empty || de.Position == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var trucksToImport = new List<Truck>();
                var newDespatcher = new Despatcher()
                {
                    Name = de.Name,
                    Position = de.Position,
                };
                foreach (var t in de.Trucks)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var newTruck = new Truck()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = (CategoryType)t.CategoryType,
                        MakeType = (MakeType)t.MakeType,
                        Despatcher = newDespatcher
                    };
                    trucksToImport.Add(newTruck);
                }
                newDespatcher.Trucks = trucksToImport;
                despatchersToImport.Add(newDespatcher); 
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, newDespatcher.Name, trucksToImport.Count));
            }
            context.Despatchers.AddRange(despatchersToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var ClientsDtos = JsonConvert
                .DeserializeObject<ImportClientDto[]>(jsonString);

            var clientsToImport = new List<Client>();

            foreach(var c in ClientsDtos)
            {
                if (!IsValid(c) || c.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var validClientsTrucks = new List<ClientTruck>();
                
                var newClient = new Client()
                {
                    Name= c.Name,
                    Nationality = c.Nationality,
                    Type = c.Type,
                };


                foreach(var ct  in c.Trucks.Distinct())
                {
                    if (!context.Trucks.Any(t => t.Id == ct))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newClientTruck = new ClientTruck()
                    {
                         Client = newClient,
                         TruckId = ct,
                    };
                    validClientsTrucks.Add(newClientTruck);
                }
                newClient.ClientsTrucks = validClientsTrucks;
                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, newClient.Name, validClientsTrucks.Count));
            }
            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}