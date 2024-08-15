using System.ComponentModel.DataAnnotations;
using static Trucks.Data.DataConstrains;
namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(DespatcherNameMaxValue)]
        public string Name { get; set; } = null!;

        public string? Position { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; } = new HashSet<Truck>();
    }
}