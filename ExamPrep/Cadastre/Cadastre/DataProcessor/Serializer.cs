using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Invoices.Utilities;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
           var properties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= new DateTime(2000,1,1))
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                    .Select(pc => pc.Citizen)
                    .OrderBy(c => c.LastName)
                    .Select(c => new
                    {
                        LastName = c.LastName,
                        MaritalStatus = c.MaritalStatus.ToString(),
                    })
                    .ToList()
                })
                .ToList();

          return  JsonConvert.SerializeObject(properties , Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertiesDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    PostalCode = p.District.PostalCode,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                })
                .ToList();

            return XmlSerializationHelper.Serialize(properties, "Properties");
        }
    }
}
