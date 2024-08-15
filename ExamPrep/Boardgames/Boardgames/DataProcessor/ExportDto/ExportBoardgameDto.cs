using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType(nameof(Boardgame))]
    public class ExportBoardgameDto
    {
        [Required]
        [XmlElement(nameof(BoardgameName))]
        public string BoardgameName { get; set; } = null!;

        [XmlElement(nameof(BoardgameYearPublished))]
        public int BoardgameYearPublished { get; set; }
    }
}
