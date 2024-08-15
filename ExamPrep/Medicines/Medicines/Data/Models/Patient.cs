using System.ComponentModel.DataAnnotations;
using Medicines.Data.Models.Enums;
using static Medicines.Data.DataConstraints;
namespace Medicines.Data.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(PatientFullNameMaxValue)]
        public string FullName { get; set; } = null!;
        public AgeGroup AgeGroup { get; set; }
        public Gender Gender { get; set; }

        public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; } = new List<PatientMedicine>();

    }
}
