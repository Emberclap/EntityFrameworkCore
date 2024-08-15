
using Theatre.Data.Models.Enums;

namespace Theatre.Data
{
    public class DataConstraints
    {
        //Theatre
        public const byte TheatreNameMinValue = 4;
        public const byte TheatreNameMaxValue = 30;
        public const sbyte TheatreNumberOfHallsMinValue = 1;
        public const sbyte TheatreNumberOfHallsMaxValue = 10;
        public const byte TheatreDirectorMinValue = 4;
        public const byte TheatreDirectorMaxValue = 30;

        //Play
        public const byte PlayTitleMinValue = 4;
        public const byte PlayTitleMaxValue = 50;
        public const float PlayRatingMinValue = 0.00f;
        public const float PlayRatingMaxValue = 10.00f;
        public const int PlayGenreMinValue = (int)Genre.Drama;
        public const int PlayGenreMaxValue = (int)Genre.Musical;
        public const int PlayDescriptionMaxValue = 700;
        public const byte PlayScreenwriterMinValue = 4;
        public const byte PlayScreenwriterMaxValue = 30;

        //Cast
        public const byte CastFullNameMinValue = 4;
        public const byte CastFullNameMaxValue = 30;
        public const string CastPhoneNumberRegex = @"^\+44-[\d]{2}-[\d]{3}-[\d]{4}$";

        //Ticket
        public const string TicketPriceMinValue = "1.00";
        public const string TicketPriceMaxValue = "100.00";
        public const sbyte TicketRowNumberMinValue = 1;
        public const sbyte TicketRowNumberMaxValue = 10;
    }
}
