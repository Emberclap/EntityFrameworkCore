using System.ComponentModel.DataAnnotations;
using static TravelAgency.Data.DataConstraints;
namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingsDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;
        [Required]
        [MinLength(CustomerFullNameMinValue), MaxLength(CustomerFullNameMaxValue)]
        public string CustomerName { get; set; } = null!;
        [Required]
        [MinLength(TourPackagePackageNameMinValue), MaxLength(TourPackagePackageNameMaxValue)]
        public string TourPackageName { get; set; } = null!;

    }
}
