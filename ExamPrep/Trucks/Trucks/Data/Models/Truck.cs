using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;
using static Trucks.Data.DataConstrains;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(TruckRegistrationNumberMaxValue)]
        public string RegistrationNumber { get; set; } = null!;
        [Required]
        [MaxLength(TruckVinNumberMaxValue)]
        public string VinNumber { get; set; } = null!;
        public int TankCapacity { get; set; }
        public int CargoCapacity { get; set; }
        public CategoryType CategoryType { get; set; }
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId  { get; set; }
        [Required]
        public Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = new HashSet<ClientTruck>();
    }
}
