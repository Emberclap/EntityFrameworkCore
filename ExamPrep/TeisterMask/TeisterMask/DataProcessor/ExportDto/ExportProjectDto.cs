using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType(nameof(Project))]
    public class ExportProjectDto
    {
        [XmlAttribute(nameof(TasksCount))]
        public int TasksCount { get; set; }
        [Required]
        [XmlElement(nameof(ProjectName))]
        public string ProjectName { get; set; } = null!;
        [Required]
        [XmlElement(nameof(HasEndDate))]
        public string HasEndDate { get; set; } = null!;
        [XmlArray(nameof(Tasks))]
        public ExportTaskDto[] Tasks { get; set; } = null!;

    }
}
