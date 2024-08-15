using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models;
using static Trucks.Data.DataConstrains;
namespace Trucks.DataProcessor.ImportDto
{
    [XmlType(nameof(Despatcher))]
    public class ImportDespatchersDto
    {
        [Required]
        [MinLength(DespatcherNameMinValue)]
        [MaxLength(DespatcherNameMaxValue)]
        public string Name { get; set; } = null!;
        [XmlElement(nameof(Position))]
        public string? Position { get; set; }
        [XmlArray(nameof(Trucks))]
        public ImportTrucksDto[] Trucks { get; set; } = null!;
    }
}
