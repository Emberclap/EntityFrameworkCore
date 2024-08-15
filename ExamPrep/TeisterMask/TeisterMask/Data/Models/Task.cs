using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeisterMask.Data.Models.Enums;
using static TeisterMask.Data.DataConstrains;
namespace TeisterMask.Data.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TaskNameMaxValue)]
        public string Name { get; set; } = null!;
        public DateTime OpenDate { get; set; }

        public DateTime DueDate { get; set; }

        public ExecutionType ExecutionType { get; set; }
        public LabelType LabelType { get; set; }
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = new HashSet<EmployeeTask>();
    }
}