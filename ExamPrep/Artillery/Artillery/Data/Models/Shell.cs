using System.ComponentModel.DataAnnotations;
using static Artillery.Data.DataConstrains;
namespace Artillery.Data.Models
{
    public class Shell
    {
        [Key]
        public int Id { get; set; }
        public double ShellWeight { get; set; }
        [Required]
        [MaxLength(ShellCaliberMaxValue)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = new List<Gun>();
    }
}
