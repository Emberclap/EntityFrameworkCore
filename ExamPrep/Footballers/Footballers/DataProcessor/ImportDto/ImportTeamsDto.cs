using System.ComponentModel.DataAnnotations;
using static Footballers.Data.DataConstraints;
namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamsDto
    {
        [Required]
        [MinLength(TeamNameMinValue)]
        [MaxLength(TeamNameMaxValue)]
        [RegularExpression(TeamNameRegex)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(TeamNationalityMinValue)]
        [MaxLength(TeamNationalityMaxValue)]
        public string Nationality { get; set; } = null!;

        public int Trophies { get; set; }
        [Required]
        public int[] Footballers { get; set; } = null!;
    }
}
