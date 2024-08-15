using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Data.DataConstrains;
namespace Cadastre.Data.Models
{
    public class Citizen
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(CitizenFirstNameMaxValue)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(CitizenLastNameMaxValue)]
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public MaritalStatus MaritalStatus { get; set; }

        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();
    }
}
