using Artillery.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType(nameof(Country))]
    public class ExportCountriesDto
    {
        [Required]
        [XmlAttribute(nameof(Country))]
        public string Country { get; set; } = null!;
        [XmlAttribute(nameof(ArmySize))]

        public int ArmySize { get; set; }
    }
}
