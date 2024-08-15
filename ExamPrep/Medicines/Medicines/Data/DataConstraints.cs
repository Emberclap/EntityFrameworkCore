using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Medicines.Data
{
    public class DataConstraints
    {
        //Pharmacy
        public const byte PharmacyNameMinValue = 2;
        public const byte PharmacyNameMaxValue = 50;
        public const byte PharmacyPhoneNumberMaxValue = 14;
        public const string PharmacyPhoneNumberRegex = @"^\([\d]{3}\)\s[\d]{3}-[\d]{4}$";


        //Medicine
        public const int MedicineNameMinValue = 3;
        public const int MedicineNameMaxValue = 150;
        public const string MedicinePriceMinValue = "0.01";
        public const string MedicinePriceMaxValue = "1000.00";
        public const byte MedicineProducerMinValue = 3;
        public const byte MedicineProducerMaxValue = 100;

        //Patient
        public const byte PatientFullNameMinValue = 5;
        public const byte PatientFullNameMaxValue = 100;
    }
}
