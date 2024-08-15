using System.ComponentModel.DataAnnotations;
using static TeisterMask.Data.DataConstrains;
namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [Required]
        [RegularExpression(EmployeeUserNameRegex)]
        [MinLength(EmployeeUserNameMinValue), MaxLength(EmployeeUserNameMaxValue)]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(EmployeePhoneRegex)]
        public string Phone { get; set; } = null!;

        public int[] Tasks { get; set; } = null!;
    }
}
