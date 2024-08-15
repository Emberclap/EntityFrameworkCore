using System.ComponentModel.DataAnnotations;
using static Medicines.Data.DataConstraints;
namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(PharmacyNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(PharmacyPhoneNumberMaxValue)]
        public string PhoneNumber { get; set; } = null!;

        public bool IsNonStop {  get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
