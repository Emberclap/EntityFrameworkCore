namespace Medicines.DataProcessor
{
    using Invoices.Utilities;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var sb = new StringBuilder();
           var patients = JsonConvert
                .DeserializeObject<ImportPatientDto[]>(jsonString);

            var patientsToImport = new List<Patient>();

            foreach(var p in patients)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newPatient = new Patient()
                {
                    FullName = p.FullName,
                    AgeGroup = p.AgeGroup,
                    Gender = p.Gender,                   
                };

                foreach(var m in p.Medicines)
                {
                    if(newPatient.PatientsMedicines.Any(pm => pm.MedicineId == m))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var patientMedicine = new PatientMedicine()
                    {
                        Patient = newPatient,
                        MedicineId = m,
                    };
                    newPatient.PatientsMedicines.Add(patientMedicine);
                }
                patientsToImport.Add(newPatient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, newPatient.FullName, newPatient.PatientsMedicines.Count));
            }
            context.Patients.AddRange(patientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            var sb = new StringBuilder();

            const string xmlRoot = "Pharmacies";

            var pharmaciesDtos = XmlSerializationHelper
                .Deserialize<ImportPharmaciesDto[]>(xmlString, xmlRoot);

            var pharmaciesToImport = new List<Pharmacy>();


            foreach(var p  in pharmaciesDtos)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                if (!bool.TryParse(p.IsNonStop, out bool result))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                var medicinesToImport = new List<Medicine>();

                foreach(var m in p.Medicines)
                {
                    bool isProductionDateValid = DateTime
                        .TryParse(m.ProductionDate,CultureInfo.InvariantCulture , DateTimeStyles.None, out DateTime productionDate);
                    bool isExpiryDateValid = DateTime
                        .TryParse(m.ExpiryDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiryDate);

                    if (!IsValid(m) || !isProductionDateValid || !isExpiryDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (medicinesToImport.Any(med => med.Name == m.Name 
                            && med.Producer == m.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    var newMedicine = new Medicine()
                    {
                        Name = m.Name,
                        Price = m.Price,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = m.Producer,
                        Category = (Category)m.Category
                    };
                    medicinesToImport.Add(newMedicine);
                }

                var pharmacy = new Pharmacy()
                {
                    IsNonStop = result,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Medicines = medicinesToImport,
                };
                pharmaciesToImport.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name,  medicinesToImport.Count));
            }
            context.Pharmacies.AddRange(pharmaciesToImport);
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
