using System.ComponentModel.DataAnnotations;
using static TeisterMask.Data.DataConstrains;
namespace TeisterMask.Data.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(EmployeeUserNameMaxValue)]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone {  get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = new HashSet<EmployeeTask>();


    }
}
