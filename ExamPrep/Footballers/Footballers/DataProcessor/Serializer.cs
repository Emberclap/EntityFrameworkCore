namespace Footballers.DataProcessor
{
    using Castle.DynamicProxy.Generators;
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                 .ToArray()
                 .Where(t => t.Footballers.Any())
                 .Select(c => new ExportCoachesDto
                 {
                     CoachName = c.Name,
                     FootballersCount = c.Footballers.Count,
                     Footballers = c.Footballers
                     .Select(f => new ExportFootballersDto
                     {
                         Name = f.Name,
                         Position = f.PositionType.ToString(),
                     })
                     .OrderBy(f => f.Name)
                     .ToArray()
                 })
                 .OrderByDescending(fc => fc.FootballersCount)
                 .ThenBy(f => f.CoachName)
                 .ToList();

            return XmlSerializationHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {

            var footballers = context.Teams
                .Where(f => f.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Select(tf => tf.Footballer)
                    .Where(tf => tf.ContractStartDate >= date)
                    .OrderByDescending(tf => tf.ContractEndDate)
                    .ThenBy(tf => tf.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Name,
                        ContractStartDate = f.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.BestSkillType.ToString(),
                        PositionType = f.PositionType.ToString(),
                    })
                    .ToList()
                })
                .OrderByDescending(f => f.Footballers.Count)
                .ThenBy(fn => fn.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(footballers, Formatting.Indented);
    
        }
    }
}
