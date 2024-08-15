using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Serialization;
using static Medicines.Data.DataConstraints;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Medicine))]
    public class ImportMedicineDto
    {
        [XmlAttribute("category")]
        [Range(0,4)]
        public int Category { get; set; }
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(MedicineNameMinValue)]
        [MaxLength(MedicineNameMaxValue)]
        public string Name { get; set; } = null!;
        [Range(typeof(decimal), MedicinePriceMinValue, MedicinePriceMaxValue)]
        public decimal Price { get; set; }
        [Required]
        public string ProductionDate { get; set; } = null!;
        [Required]
        public string ExpiryDate { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Producer))]
        [MinLength(MedicineProducerMinValue)]
        [MaxLength(MedicineProducerMaxValue)]
        public string Producer { get; set; } = null!;

    }
}