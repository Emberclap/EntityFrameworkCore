using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType(nameof(Property))]
    public class ExportPropertiesDto
    {
        [Required]
        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [XmlElement(nameof(PropertyIdentifier))]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement(nameof(Area))]
        public int Area { get; set; }

        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
