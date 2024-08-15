using Footballers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType(nameof(Coach))]
    public class ExportCoachesDto
    {
        [XmlAttribute(nameof(FootballersCount))]
        public int FootballersCount { get; set; }
        [XmlElement(nameof(CoachName))]
        public string CoachName { get; set; } = null!;
        [XmlArray(nameof(Footballers))]
        public ExportFootballersDto[] Footballers { get; set; } = null!;
    }
}
