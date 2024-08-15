using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data
{
    public class DataConstrains
    {
        //BoardGame
        public const byte BoardgameNameMinValue = 10;
        public const byte BoardgameNameMaxValue = 20;
        public const double BoardgameRatingMinValue = 1.00d;
        public const double BoardgameRatingMaxValue = 10.00d;
        public const int BoardgameYearPublishedMinValue = 2018;
        public const int BoardgameYearPublishedMaxValue = 2023;


        //Creator
        public const byte CreatorFirstNameMinValue = 2;
        public const byte CreatorFirstNameMaxValue = 7;
        public const byte CreatorLastNameMinValue = 2;
        public const byte CreatorLastNameMaxValue = 7;

        //Seller
        public const byte SellerNameMinValue = 5;
        public const byte SellerNameMaxValue = 20;
        public const byte SellerAddressMinValue = 2;
        public const byte SellerAddressMaxValue = 30;
    }
}
