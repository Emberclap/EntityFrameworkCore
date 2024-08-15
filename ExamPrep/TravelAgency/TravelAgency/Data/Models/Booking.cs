using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        [Required]
        public Customer Customer { get; set; } = null!;
        [ForeignKey(nameof(TourPackage))]
        public int TourPackageId { get; set; }
        [Required]
        public TourPackage TourPackage { get; set; } = null!;
    }
}