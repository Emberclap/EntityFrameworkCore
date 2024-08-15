namespace Medicines.DataProcessor
{
    using Invoices.Utilities;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.DataProcessor.ImportDtos;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime givenDate;

            if (!DateTime.TryParse(date, out givenDate))
            {
                throw new ArgumentException("Invalid date format!");
            }

            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate >= givenDate))
                .Select(p => new ExportPatientWithMedicineDto
                {
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup,
                    Gender = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                    .Where(pm => pm.Medicine.ProductionDate >= givenDate)
                    .Select(pm => pm.Medicine)
                    .OrderByDescending(m => m.ExpiryDate)
                    .ThenBy(m => m.Price)
                    .Select(m => new ExportMedicineDto
                    {
                        Name = m.Name,
                        Price = m.Price.ToString("F2"),
                        Category = m.Category.ToString().ToLower(),
                        Producer = m.Producer,
                        BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd")
                    })
                    .ToArray()
                })
                .OrderByDescending(p => p.Medicines.Length)
                .ThenBy(p => p.Name)
                .ToList();

            return XmlSerializationHelper.Serialize(patients, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(m => (int)m.Category == medicineCategory && m.Pharmacy.IsNonStop == true)
                .OrderBy(p => p.Price)
                .ThenBy(p => p.Name)
                .Select(m => new
                {
                    m.Name,
                    Price = m.Price.ToString("F2"),
                    Pharmacy = new 
                    {
                        m.Pharmacy.Name,
                        m.Pharmacy.PhoneNumber
                    }
                })
                .ToList();

            return JsonConvert.SerializeObject(medicines, Formatting.Indented);

        }
    }
}
