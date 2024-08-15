using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;
using static Boardgames.Data.DataConstrains;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Boardgame))]
    public class ImportBoardgameDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MaxLength(BoardgameNameMaxValue)]
        [MinLength(BoardgameNameMinValue)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Rating))]
        [Range(BoardgameRatingMinValue, BoardgameRatingMaxValue)]
        public double Rating { get; set; }
        [XmlElement(nameof(YearPublished))]
        [Range(BoardgameYearPublishedMinValue, BoardgameYearPublishedMaxValue)]
        public int YearPublished { get; set; }
        [XmlElement(nameof(CategoryType))]
        public int CategoryType { get; set; }
        [Required]
        [XmlElement(nameof(Mechanics))]
        public string Mechanics { get; set; } = null!;
    }
}
