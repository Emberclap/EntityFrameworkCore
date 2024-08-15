using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using static Artillery.Data.DataConstrains;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunsDto
    {
        public int ManufacturerId { get; set; }
        [Range(GunGunWeightMinValue, GunGunWeightMaxValue)]
        public int GunWeight { get; set; }
        [Range(GunBarrelLengthMinValue, GunBarrelLengthMaxValue)]
        public double BarrelLength { get; set; }

        public int NumberBuild { get; set; }
        [Range(GunRangeMinValue, GunRangeMaxValue)]
        public int Range { get; set; }
        public string GunType { get; set; } = null!;
        public int ShellId { get; set; }

        public ImportCountryGunsDto[] Countries { get; set; } = null!;

    }
    public class ImportCountryGunsDto
    {
        public int Id { get; set; }
    }
}
