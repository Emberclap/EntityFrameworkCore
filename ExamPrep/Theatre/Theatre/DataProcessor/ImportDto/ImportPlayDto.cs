using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;
using static Theatre.Data.DataConstraints;
namespace Theatre.DataProcessor.ImportDto
{
    [XmlType(nameof(Play))]
    public class ImportPlayDto
    {
        [Required]
        [XmlElement(nameof(Title))]
        [MinLength(PlayTitleMinValue), MaxLength(PlayTitleMaxValue)]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Duration))]
        public string Duration { get; set; } = null!;

        [XmlElement(nameof(Raiting))]
        [Range(PlayRatingMinValue, PlayRatingMaxValue)]
        public float Raiting { get; set; }

        [XmlElement(nameof(Genre))]
        public string Genre { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Description))]
        [MaxLength(PlayDescriptionMaxValue)]
        public string Description { get; set; } = null!;

        [Required]
        [XmlElement(nameof(Screenwriter))]
        [MinLength(PlayScreenwriterMinValue),MaxLength(PlayScreenwriterMaxValue)]
        public string Screenwriter { get; set; } = null!;
    }
}
