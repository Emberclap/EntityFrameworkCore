namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using System.Text.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                    .ToArray()
                    .Select(c => new ExportCreatorsDto()
                    {
                        CreatorName = $"{c.FirstName} {c.LastName}",
                        BoardgamesCount = c.Boardgames.Count(),
                        Boardgames = c.Boardgames
                            .Select(b => new ExportBoardgameDto
                            {
                                BoardgameName = b.Name,
                                BoardgameYearPublished = b.YearPublished,
                            })
                            .OrderBy(b=> b.BoardgameName)
                            .ToArray()
                    })
                    .OrderByDescending(b=> b.Boardgames.Count())
                    .ThenBy(c => c.CreatorName)
                    .ToList();

            return XmlSerializationHelper.Serialize(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year 
                        && b.Boardgame.Rating <= rating))
                .Select(s => new 
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Select(b => b.Boardgame)
                        .Where(b => b.YearPublished >= year && b.Rating <= rating)
                    .Select(b => new
                    {
                        Name = b.Name,
                        b.Rating,
                        b.Mechanics,
                        Category = b.CategoryType.ToString(),
                    })
                    .OrderByDescending(b => b.Rating)
                    .ThenBy(b => b.Name)
                    .ToList()
                })
                .OrderByDescending(b => b.Boardgames.Count)
                .ThenBy(b=> b.Name)
                .Take(5)
                .ToList();

            return JsonSerializer.Serialize(sellers, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}