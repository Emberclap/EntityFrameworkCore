namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Json;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Invoices.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var sb = new StringBuilder();
            const string xmlRoot = "Creators";

            var creatorDtos = XmlSerializationHelper
                .Deserialize<ImportCreatorsDto[]>(xmlString, xmlRoot);

            var creatorsToImport = new List<Creator>();

            foreach (var c in creatorDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var boardgamesToImport = new List<Boardgame>();
                foreach (var b in c.Boardgames)
                {
                    if (!IsValid(b))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var boardgame = new Boardgame()
                    {
                        Name = b.Name,
                        Rating = b.Rating,
                        YearPublished = b.YearPublished,
                        CategoryType = (CategoryType)b.CategoryType,
                        Mechanics = b.Mechanics,
                    };
                    boardgamesToImport.Add(boardgame);
                }

                var creator = new Creator()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Boardgames = boardgamesToImport
                };
                creatorsToImport.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, c.FirstName, c.LastName, boardgamesToImport.Count));
            }
            context.Creators.AddRange(creatorsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var sellersDtos = JsonSerializer
                .Deserialize<ImportSellersDto[]>(jsonString);
            var boardgamesIds = context.Boardgames
                .Select(b => b.Id)
                .ToList();

            var sellersToImport = new List<Seller>();

            foreach (var s in sellersDtos)
            {
                if (!IsValid(s))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var seller = new Seller()
                {
                    Name = s.Name,
                    Address = s.Address,
                    Country = s.Country,
                    Website = s.Website,
                };
                var boardgamesSellerToImport = new List<BoardgameSeller>();
                foreach (var b in s.Boardgames.Distinct())
                {
                    if (!boardgamesIds.Contains(b))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var newBoardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = b,
                    };

                    boardgamesSellerToImport.Add(newBoardgameSeller);
                }
                seller.BoardgamesSellers = boardgamesSellerToImport;
                sellersToImport.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, boardgamesSellerToImport.Count));
            }
            context.Sellers.AddRange(sellersToImport);
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
