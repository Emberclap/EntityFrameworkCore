using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data
{
    public class DataConstraints
    {
        //Customer
        public const byte CustomerFullNameMinValue = 4;
        public const byte CustomerFullNameMaxValue = 60;
        public const byte CustomerEmailMinValue = 6;
        public const byte CustomerEmailMaxValue = 50;
        public const byte CustomerPhoneNumberMaxValue = 13;
        public const string CustomerPhoneNumberRegex = @"\+[\d]{12}$";

        //Guide
        public const byte GuideFullNameMinValue = 4;
        public const byte GuideFullNameMaxValue = 60;
        public const int GuideLanguageMinValue = (int)Language.English;
        public const int GuideLanguageMaxValue = (int)Language.Russian;

        //TourPackage
        public const byte TourPackagePackageNameMinValue = 2;
        public const byte TourPackagePackageNameMaxValue = 40;
        public const byte TourPackageDescriptionMaxValue = 200;

    }
}
