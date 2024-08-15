using System.ComponentModel.DataAnnotations;
using static Theatre.Data.DataConstraints;
namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreDto
    {
        [Required]
        [MinLength(TheatreNameMinValue),MaxLength(TheatreNameMaxValue)]
        public string Name { get; set; } = null!;
        [Required]
        [Range(TheatreNumberOfHallsMinValue, TheatreNumberOfHallsMaxValue)]
        public sbyte NumberOfHalls { get; set; }
        [Required]
        [MinLength(TheatreDirectorMinValue),MaxLength(TheatreDirectorMaxValue)]
        public string Director { get; set; } = null!;
        [Required]
        public ImportTicketDto[] Tickets { get; set; } = null!;
    }
}
