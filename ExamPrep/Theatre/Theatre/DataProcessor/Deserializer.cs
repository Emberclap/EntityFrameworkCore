namespace Theatre.DataProcessor
{
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var playDtos = XmlSerializationHelper
                .Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            var playsToImport = new List<Play>();

            foreach(var p in playDtos)
            {
                bool isDurationValid = TimeSpan
                    .TryParseExact(p.Duration, "c",CultureInfo.InvariantCulture, out var duration);
                bool isGenreValid = Enum
                    .TryParse(p.Genre, out Genre genreType);

                if (!IsValid(p)|| !isDurationValid || duration < new TimeSpan(1,0,0)|| !isGenreValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newPlay = new Play
                {
                    Title = p.Title,
                    Duration = duration,
                    Rating = p.Raiting,
                    Genre = genreType,
                    Description = p.Description,
                    Screenwriter = p.Screenwriter,
                };
                playsToImport.Add(newPlay);
                sb.AppendLine(string.Format(SuccessfulImportPlay, newPlay.Title, genreType, newPlay.Rating));
            }
            context.Plays.AddRange(playsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var castsDto = XmlSerializationHelper
                .Deserialize<ImportCastDto[]>(xmlString, "Casts");

            var castsToImport = new List<Cast>();

            foreach(var c in castsDto )
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                }

                var newCast = new Cast
                {
                    FullName = c.FullName,
                    IsMainCharacter = c.IsMainCharacter,
                    PhoneNumber = c.PhoneNumber,
                    PlayId = c.PlayId,
                };
                castsToImport.Add(newCast);
                sb.AppendLine(string.Format(SuccessfulImportActor, newCast.FullName, newCast.IsMainCharacter? "main" : "lesser")) ;
            }
            context.Casts.AddRange(castsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var theatreDto = JsonConvert
                .DeserializeObject<ImportTheatreDto[]>(jsonString);

            var theatreToImport = new List<Theatre>();

            foreach(var t in theatreDto)
            {
                if (!IsValid(t))
                {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                }

                var newTheatre = new Theatre
                {
                    Name = t.Name,
                    NumberOfHalls = t.NumberOfHalls,
                    Director = t.Director,
                };
                foreach(var ticket in t.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var newTicket = new Ticket
                    {
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber,
                        PlayId = ticket.PlayId,
                    };
                    newTheatre.Tickets.Add(newTicket);
                }
                theatreToImport.Add(newTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, newTheatre.Name, newTheatre.Tickets.Count));
            }
            context.Theatres.AddRange(theatreToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
