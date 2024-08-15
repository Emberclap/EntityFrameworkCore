using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType(nameof(Footballer))]
    public class ExportFootballersDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;
        [XmlElement(nameof(Position))]
        public string Position { get; set; } = null!;
    }
}