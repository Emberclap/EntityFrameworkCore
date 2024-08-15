using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data
{
    public class DataConstrains
    {
        public const byte DistrictNameMinValue = 2;
        public const byte DistrictNameMaxValue = 80;



        public const byte PropertyMinIdentifierValue = 16;
        public const byte PropertyMaxIdentifierValue = 20;
        public const int PropertyDetailsMinValue = 5;
        public const int PropertyDetailsMaxValue = 500;
        public const int PropertyAddressMinValue = 5;
        public const int PropertyAddressMaxValue = 200;

        public const byte CitizenFirstNameMinValue = 2;
        public const byte CitizenFirstNameMaxValue = 30;
        public const byte CitizenLastNameMinValue = 2;
        public const byte CitizenLastNameMaxValue = 30;
    }
}
