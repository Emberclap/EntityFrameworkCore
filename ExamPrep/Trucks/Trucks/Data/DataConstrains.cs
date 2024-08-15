using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;

namespace Trucks.Data
{
    public class DataConstrains
    {
        //Truck
        public const byte TruckRegistrationNumberMaxValue = 8;
        public const string TruckRegistrationNumberRegex = @"^[A-Z]{2}\d{4}[A-Z]{2}$";
        public const byte TruckVinNumberMinValue = 17;
        public const byte TruckVinNumberMaxValue = 17;
        public const int TruckTankCapacityMinValue = 950;
        public const int TruckTankCapacityMaxValue = 1420;
        public const int TruckCargoCapacityMinValue = 5000;
        public const int TruckCargoCapacityMaxValue = 29000;
        public const int TruckCategoryTypeMinValue = (int)CategoryType.Flatbed;
        public const int TruckCategoryTypeMaxValue = (int)CategoryType.Semi;
        public const int TruckMakeTypeMinValue = (int)MakeType.Daf;
        public const int TruckMakeTypeMaxValue = (int)MakeType.Volvo;

        //Client
        public const byte ClientNameMinValue = 3;
        public const byte ClientNameMaxValue = 40;
        public const byte ClientNationalityMinValue = 2;
        public const byte ClientNationalityMaxValue = 40;

        //Dispatcher
        public const byte DespatcherNameMinValue = 2;
        public const byte DespatcherNameMaxValue = 40;
    }
}
