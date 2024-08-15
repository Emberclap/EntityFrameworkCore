using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Medicines.Data.DataConstraints;
namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(PatientFullNameMinValue)]
        [MaxLength(PatientFullNameMaxValue)]
        public string FullName { get; set; } = null!;
        [Range(0,2)]
        public AgeGroup AgeGroup { get; set; }
        [Range(0, 1)]
        public Gender Gender { get; set; }
        public int[] Medicines { get; set; } = null!;
    }
}
