namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var countriesDtos = XmlSerializationHelper
                .Deserialize<ImportCountriesDto[]>(xmlString, "Countries");
            var countriesToImport = new List<Country>();

            foreach(var c in countriesDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newCountry = new Country
                {
                    CountryName = c.CountryName,
                    ArmySize = c.ArmySize,
                };
                countriesToImport.Add(newCountry);
                sb.AppendLine(string.Format(SuccessfulImportCountry, newCountry.CountryName , newCountry.ArmySize));
            }
            context.Countries.AddRange(countriesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var manufacturersDtos = XmlSerializationHelper
                .Deserialize<ImportManufacturersDto[]>(xmlString, "Manufacturers");

            var manufacturersToImport = new List<Manufacturer>();

            foreach (var c in manufacturersDtos)
            {
                if (!IsValid(c) || manufacturersToImport.Any(n => n.ManufacturerName == c.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var tokens = c.Founded.Split(", ");
                string address = tokens[tokens.Length-2] + ", " + tokens.LastOrDefault();


                var newManufacturer = new Manufacturer
                {
                    ManufacturerName = c.ManufacturerName,
                    Founded = c.Founded
                };
                manufacturersToImport.Add(newManufacturer);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, newManufacturer.ManufacturerName, address));
            }
            context.Manufacturers.AddRange(manufacturersToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var shellsDtos = XmlSerializationHelper
                .Deserialize<ImportShellsDto[]>(xmlString, "Shells");

            var shellsToImport = new List<Shell>();

            foreach (var c in shellsDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newShell = new Shell
                {
                   ShellWeight = c.ShellWeight,
                   Caliber = c.Caliber,
                };
                shellsToImport.Add(newShell);
                sb.AppendLine(string.Format(SuccessfulImportShell, newShell.Caliber ,newShell.ShellWeight ));
            }
            context.Shells.AddRange(shellsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            var gunsDtos = JsonConvert.DeserializeObject<ImportGunsDto[]>(jsonString, settings); ;
            var gunsToImport = new List<Gun>(); 
            
            foreach(var g in gunsDtos)
            {
                var isGuntypeValid = Enum.TryParse(g.GunType, out GunType gunType);

                if (!IsValid(g) || !isGuntypeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newGun = new Gun
                {
                    ManufacturerId = g.ManufacturerId,
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    NumberBuild = g.NumberBuild,
                    Range = g.Range,
                    GunType = gunType,
                    ShellId = g.ShellId,
                };
                var countriesGuns = new List<CountryGun>();

                foreach(var c in g.Countries)
                {
                    var newCountryGun = new CountryGun
                    {
                        CountryId = c.Id,
                        Gun = newGun
                    };
                    countriesGuns.Add(newCountryGun);
                }
                newGun.CountriesGuns = countriesGuns;
                gunsToImport.Add(newGun);
                sb.AppendLine(string.Format(SuccessfulImportGun, newGun.GunType, newGun.GunWeight , newGun.BarrelLength));
            }
            context.Guns.AddRange(gunsToImport);
            context.SaveChanges();

            return sb.ToString().ToString();

        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}