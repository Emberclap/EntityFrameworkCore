using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data
{
    public class DataConstrains
    {
        //Country
        public const byte CountryCountryNameMinValue = 4;
        public const byte CountryCountryNameMaxValue = 60;
        public const int CountryArmySizeMinValue = 50_000;
        public const int CountryArmySizeMaxValue = 10_000_000;

        //Manufacturer
        public const byte ManufacturerNameMinValue = 4;
        public const byte ManufacturerNameMaxValue = 40;
        public const byte ManufacturerFoundedMinValue = 10;
        public const byte ManufacturerFoundedMaxValue = 100;

        //Shell
        public const double ShellShellWeightMinValue = 2d;
        public const double ShellShellWeightMaxValue = 1_680d;
        public const byte ShellCaliberMinValue = 4;
        public const byte ShellCaliberMaxValue = 30;

        //Gun
        public const int GunGunWeightMinValue = 100;
        public const int GunGunWeightMaxValue = 1_350_000;
        public const double GunBarrelLengthMinValue = 2.00d;
        public const double GunBarrelLengthMaxValue = 35.00d;
        public const int GunRangeMinValue = 1;
        public const int GunRangeMaxValue = 100_000;
    }
}
