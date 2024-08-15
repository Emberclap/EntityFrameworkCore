namespace Theatre.DataProcessor
{
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context.Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(r => r.RowNumber >= 1 && r.RowNumber <= 5).Sum(p => p.Price),
                    Tickets = t.Tickets
                        .Where(r => r.RowNumber >= 1 && r.RowNumber <= 5)
                    .Select(tic => new
                    {
                        Price = tic.Price,
                        RowNumber = tic.RowNumber,
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(h => h.Halls)
                .ThenBy(h => h.Name)
                .ToList();

            return JsonConvert.SerializeObject(theaters , Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var plays = context.Plays
                .Where(p => p.Rating <= raiting)
                .OrderBy(t => t.Title)
                .ThenByDescending(g => g.Genre)
                .Select(p => new ExportPlayDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating =  p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                        .Where(c => c.IsMainCharacter)
                        .Select(c => new ExportActorDto
                        {
                            FullName = c.FullName,
                            MainCharacter = $"Plays main character in '{p.Title}'."
                        })
                        .OrderByDescending(n => n.FullName)
                        .ToArray()
                })
                .ToList();

            return XmlSerializationHelper.Serialize(plays, "Plays");
        }
    }
}
