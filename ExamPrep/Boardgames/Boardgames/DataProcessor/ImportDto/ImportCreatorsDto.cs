using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Boardgames.Data.DataConstrains;
namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Creator))]
    public class ImportCreatorsDto
    {
        [Required]
        [XmlElement(nameof(FirstName))]
        [MaxLength(CreatorFirstNameMaxValue)]
        [MinLength(CreatorFirstNameMinValue)]
        public string FirstName { get; set; } = null!;
        [Required]
        [XmlElement(nameof(LastName))]
        [MaxLength(CreatorLastNameMaxValue)]
        [MinLength(CreatorLastNameMinValue)]
        public string LastName { get; set; } = null!;
        [XmlArray(nameof(Boardgames))]
        public ImportBoardgameDto[] Boardgames { get; set; } = null!;
    }
}
