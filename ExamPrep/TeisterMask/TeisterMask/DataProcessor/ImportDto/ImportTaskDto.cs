using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;
using static TeisterMask.Data.DataConstrains;
namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType(nameof(Task))]
    public class ImportTaskDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(TaskNameMinValue)]
        [MaxLength(TaskNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; } = null!;
        [Required]
        [XmlElement(nameof(DueDate))]
        public string DueDate { get; set; } = null!;

        [XmlElement(nameof(ExecutionType))]
        [Range(0,3)]
        public int ExecutionType { get; set; }

        [XmlElement(nameof(LabelType))]
        [Range(0, 4)]
        public int LabelType { get; set; }
    }
}
