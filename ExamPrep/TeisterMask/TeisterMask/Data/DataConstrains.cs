using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.Data
{
    public class DataConstrains
    {
        //Employee
        public const byte EmployeeUserNameMinValue = 3;
        public const byte EmployeeUserNameMaxValue = 40;
        public const string EmployeeUserNameRegex = @"^[a-zA-Z\d]+$";
        public const string EmployeePhoneRegex = @"^[\d]{3}-[\d]{3}-[\d]{4}$";

        //Project
        public const byte ProjectNameMinValue = 2;
        public const byte ProjectNameMaxValue = 40;

        //Task
        public const byte TaskNameMinValue = 2;
        public const byte TaskNameMaxValue = 40;

    }
}
