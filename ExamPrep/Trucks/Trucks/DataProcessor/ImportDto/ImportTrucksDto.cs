using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using static Trucks.Data.DataConstrains;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType(nameof(Truck))]
    public class ImportTrucksDto
    {
        [Required]
        [XmlElement(nameof(RegistrationNumber))]
        [RegularExpression(TruckRegistrationNumberRegex)]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [XmlElement(nameof(VinNumber))]
        [MinLength(TruckVinNumberMinValue)]
        [MaxLength(TruckVinNumberMaxValue)]
        public string VinNumber { get; set; } = null!;

        [XmlElement(nameof(TankCapacity))]
        [Range(TruckTankCapacityMinValue, TruckTankCapacityMaxValue)]
        public int TankCapacity { get; set; }

        [XmlElement(nameof(CargoCapacity))]
        [Range(TruckCargoCapacityMinValue, TruckCargoCapacityMaxValue)]
        public int CargoCapacity { get; set; }

        [XmlElement(nameof(CategoryType))]
        [Range(TruckCategoryTypeMinValue, TruckCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [XmlElement(nameof(MakeType))]
        [Range(TruckMakeTypeMinValue, TruckMakeTypeMaxValue)]
        public int MakeType { get; set; }

    }
}