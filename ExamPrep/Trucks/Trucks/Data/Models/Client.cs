using System.ComponentModel.DataAnnotations;
using static Trucks.Data.DataConstrains;
namespace Trucks.Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ClientNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(ClientNationalityMaxValue)]
        public string Nationality { get; set; } = null!;
        [Required]
        public string Type { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = new HashSet<ClientTruck>();
    }
}
