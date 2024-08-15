using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Medicines.Data.Models.Enums;
using static Medicines.Data.DataConstraints;
namespace Medicines.Data.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(MedicineNameMaxValue)]
        public string Name { get; set; } = null!;
        
        public decimal Price { get; set; }
        
        public Category Category { get; set; }

        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        [Required]
        [MaxLength(MedicineProducerMaxValue)]
        public string Producer { get; set; } = null!;
        [ForeignKey(nameof(Pharmacy))]
        public int PharmacyId { get; set; }
        [Required]
        public Pharmacy Pharmacy { get; set; } = null!;

        public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; }  = new List<PatientMedicine>();


    }
}