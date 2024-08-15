namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Text.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            var sb = new StringBuilder();
            const string xmlRoot = "Districts";

            var districtDtos = XmlSerializationHelper
                .Deserialize<ImportDistrictsDto[]>(xmlDocument, xmlRoot);

            var districtsToImport = new List<District>();

            var propertyPIs = dbContext.Properties
                .Select(pi => pi.PropertyIdentifier)
                .ToList();
            var districtsAddresses = dbContext.Properties
                .Select(a => a.Address)
                .ToList();

            foreach (var d in districtDtos)
            {
                if (!IsValid(d) || dbContext.Districts.Any(dn => dn.Name == d.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var propertiesToImport = new List<Property>();

                foreach (var p in d.Properties)
                {
                    DateTime
                        .TryParse(p.DateOfAcquisition, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfAcquisition);

                    if (!IsValid(p))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (districtsAddresses.Contains(p.Address)
                        || propertiesToImport.Any(pi => pi.Address == p.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (propertyPIs.Contains(p.PropertyIdentifier)
                        || propertiesToImport.Any(pi => pi.PropertyIdentifier == p.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var property = new Property()
                    {
                        PropertyIdentifier = p.PropertyIdentifier,
                        Area = p.Area,
                        Details = p.Details,
                        Address = p.Address,
                        DateOfAcquisition = dateOfAcquisition
                    };
                    propertiesToImport.Add(property);
                }
                var newDistrict = new District()
                {
                    Name = d.Name,
                    PostalCode = d.PostalCode,
                    Properties = propertiesToImport,
                    Region = (Region)Enum.Parse(typeof(Region), d.Region)
                };
                districtsToImport.Add(newDistrict);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, d.Name, propertiesToImport.Count));
            }
            dbContext.Districts.AddRange(districtsToImport);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            var sb = new StringBuilder();

            var citizens = JsonConvert
                .DeserializeObject<ImportCitizensDto[]>(jsonDocument);

            var citizensToImport = new List<Citizen>();

            foreach (var c in citizens)
            {

                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime
                       .TryParse(c.BirthDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate);


                var newCitizen = new Citizen()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    BirthDate = birthDate,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), c.MaritalStatus)
                };
                foreach (var p in c.Properties)
                {

                    var newPropertiesCitizen = new PropertyCitizen()
                    {
                        Citizen = newCitizen,
                        PropertyId = p
                    };
                    newCitizen.PropertiesCitizens.Add(newPropertiesCitizen);
                }
                citizensToImport.Add(newCitizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, c.FirstName , c.LastName, newCitizen.PropertiesCitizens.Count));
            }
            dbContext.Citizens.AddRange(citizensToImport);
            dbContext.SaveChanges();

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


