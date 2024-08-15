using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType(nameof(Gun))]
    public class ExportGunsDto
    {
        [XmlAttribute(nameof(Manufacturer))]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute(nameof(GunType))]
        public GunType GunType { get; set; }

        [XmlAttribute(nameof(GunWeight))]
        public int GunWeight { get; set; }
        [XmlAttribute(nameof(BarrelLength))]
        public double BarrelLength { get; set; }

        [XmlAttribute(nameof(Range))]
        public int Range { get; set; }
        [XmlArray(nameof(Countries))]
        public ExportCountriesDto[] Countries { get; set; } = null!;

    }
}
