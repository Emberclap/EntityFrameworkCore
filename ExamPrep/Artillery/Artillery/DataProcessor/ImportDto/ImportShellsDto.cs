using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Artillery.Data.DataConstrains;
namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(nameof(Shell))]
    public class ImportShellsDto
    {
        [XmlElement(nameof(ShellWeight))]
        [Range(ShellShellWeightMinValue, ShellShellWeightMaxValue)]
        public double ShellWeight { get; set; }
        [Required]
        [MinLength(ShellCaliberMinValue)]
        [MaxLength(ShellCaliberMaxValue)]
        public string Caliber { get; set; } = null!;
    }
}
