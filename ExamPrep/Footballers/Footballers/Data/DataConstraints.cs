using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data
{
    public class DataConstraints
    {
        //Footballer
        public const byte FootballerNameMinValue = 2;
        public const byte FootballerNameMaxValue = 40;
        public const byte FootballerPositionTypeMinValue = (int)PositionType.Goalkeeper;
        public const byte FootballerPositionTypeMaxValue = (int)PositionType.Forward;
        public const byte FootballerBestSkillTypeMinValue = (int)BestSkillType.Defence;
        public const byte FootballerBestSkillTypeMaxValue = (int)BestSkillType.Speed;

        //Coach
        public const byte CoachNameMinValue = 2;
        public const byte CoachNameMaxValue = 40;

        //Team
        public const string TeamNameRegex = @"^[\w\s\d\-\.]+$";
        public const byte TeamNameMinValue = 3;
        public const byte TeamNameMaxValue = 40;
        public const byte TeamNationalityMinValue = 2;
        public const byte TeamNationalityMaxValue = 40;
    }
}
