using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType(nameof(Despatcher))]
    public class ExportDespatchersDto
    {
        [XmlAttribute(nameof(TrucksCount))]
        public int TrucksCount { get; set; }
        [XmlElement(nameof(DespatcherName))]
        public string DespatcherName { get; set; } = null!;
        [XmlArray(nameof(Trucks))]
        public ExportTrucksDto[] Trucks { get; set; } = null!;
    }
}
