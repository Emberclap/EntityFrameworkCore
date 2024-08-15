using System.ComponentModel.DataAnnotations;
using static Trucks.Data.DataConstrains;
namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDto
    {
        [Required]
        [MinLength(ClientNameMinValue)]
        [MaxLength(ClientNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(ClientNationalityMinValue)]
        [MaxLength(ClientNationalityMaxValue)]
        public string Nationality { get; set; } = null!;
        [Required]
        public string? Type { get; set; }

        public int[] Trucks { get; set; } = null!;
    }
}
