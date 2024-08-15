using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType(nameof(Play))]
    public class ExportPlayDto
    {

        [Required]
        [XmlAttribute(nameof(Title))]
        public string Title { get; set; } = null!;

        [Required]
        [XmlAttribute(nameof(Duration))]
        public string Duration { get; set; } = null!;

        [XmlAttribute(nameof(Rating))]
        public string Rating { get; set; } = null!;

        [XmlAttribute(nameof(Genre))]
        public string Genre { get; set; } = null!;
        [XmlArray(nameof(Actors))]
        public ExportActorDto[] Actors { get; set; } = null!;
    }
}
