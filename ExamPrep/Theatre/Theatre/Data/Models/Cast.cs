using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = null!;

        public bool IsMainCharacter { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }
        [Required]
        public Play Play { get; set; } = null!;
    }
}