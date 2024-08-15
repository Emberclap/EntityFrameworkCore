using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models;
using static TeisterMask.Data.DataConstrains;
namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType(nameof(Project))]
    public class ImportProjectsDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(ProjectNameMinValue)]
        [MaxLength(ProjectNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; } = null!;
        [XmlElement(nameof(DueDate))]
        public string? DueDate { get; set; }
        [XmlArray(nameof(Tasks))]
        public ImportTaskDto[] Tasks { get; set; } = null!;
    }
}
