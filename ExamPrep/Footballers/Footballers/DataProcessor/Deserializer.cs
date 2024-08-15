namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var sb = new StringBuilder();
            const string xmlRool = "Coaches";

            var coachDtos = XmlSerializationHelper
                .Deserialize<ImportCoachesDto[]>(xmlString, xmlRool);

            var coachesToImport = new List<Coach>();

            foreach (var c in coachDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validFootballers = new List<Footballer>();

                foreach(var f in c.Footballers)
                {
                    bool isStartDateValid = DateTime
                        .TryParseExact(f.ContractStartDate , "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None , out DateTime startDate);
                    bool isEndDateValid = DateTime
                    .TryParseExact(f.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);

                    if (!IsValid(f) || !isStartDateValid || !isEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (startDate > endDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newFootballer = new Footballer()
                    {
                        Name = f.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = (BestSkillType)f.BestSkillType,
                        PositionType = (PositionType)f.PositionType,
                    };
                    validFootballers.Add(newFootballer);
                }

                var newCoach = new Coach()
                {
                    Name= c.Name,
                    Nationality = c.Nationality,
                    Footballers = validFootballers
                };
                coachesToImport.Add(newCoach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, newCoach.Name, validFootballers.Count));
            }
            context.Coaches.AddRange(coachesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
           var sb = new StringBuilder();

            var teamDtos = JsonConvert.DeserializeObject<ImportTeamsDto[]>(jsonString);

            var teamsToImport = new List<Team>();

            foreach (var t in teamDtos)
            {
                if (!IsValid(t) || t.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var validTeamsFootballers = new List<TeamFootballer>();

                var newTeam = new Team
                {
                    Name = t.Name,
                    Nationality= t.Nationality,
                    Trophies = t.Trophies,
                };

                foreach(var f in t.Footballers.Distinct())
                {
                    if (!context.Footballers.Any(i => i.Id == f))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newTeamsFootballers = new TeamFootballer
                    {
                        Team = newTeam,
                        FootballerId = f,
                    };
                    validTeamsFootballers.Add(newTeamsFootballers);
                }
                newTeam.TeamsFootballers = validTeamsFootballers;
                teamsToImport.Add(newTeam);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, newTeam.Name, validTeamsFootballers.Count));
            }
            context.Teams.AddRange(teamsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
