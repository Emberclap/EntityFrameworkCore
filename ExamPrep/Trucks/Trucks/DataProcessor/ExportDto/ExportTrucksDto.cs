using System.Xml.Serialization;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType(nameof(Truck))]
    public class ExportTrucksDto
    {
        [XmlElement(nameof(RegistrationNumber))]
        public string RegistrationNumber { get; set; } = null!;
        [XmlElement(nameof(Make))]
        public MakeType Make { get; set; }
    }
}