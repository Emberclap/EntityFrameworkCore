using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType(nameof(Task))]
    public class ExportTaskDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;
        [XmlElement(nameof(Label))]
        public string Label { get; set; } = null!;
    }
}