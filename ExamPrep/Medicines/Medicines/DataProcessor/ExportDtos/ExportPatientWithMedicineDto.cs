using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType(nameof(Patient))]
    public class ExportPatientWithMedicineDto
    {
        [XmlAttribute(nameof(Gender))]
        public string Gender { get; set; } = null!;
        [Required]
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;
        [XmlElement(nameof(AgeGroup))]
        public AgeGroup AgeGroup { get; set; }
        [XmlArray(nameof(Medicines))]
        public ExportMedicineDto[] Medicines { get; set; } = null!;
    }
}
