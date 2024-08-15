namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
 

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            var context = new BookShopContext();

            //Console.WriteLine(GetBooksByAgeRestriction(context, "miNor"));

            //Console.WriteLine(GetGoldenBooks(context));

            //Console.WriteLine(GetBooksByPrice(context));

            //Console.WriteLine(GetBooksNotReleasedIn(context, 1998));

            //Console.WriteLine(GetBooksByCategory(context, "horror mystery drama"));

            //Console.WriteLine(GetBooksReleasedBefore(context, "12-04-1992"));

            Console.WriteLine(GetAuthorNamesEndingIn(context, "e"));

            //Console.WriteLine(GetBookTitlesContaining(context, "sK"));

            //Console.WriteLine(GetBooksByAuthor(context, "po"));

            //Console.WriteLine(CountBooks(context, 12));

            //Console.WriteLine(CountCopiesByAuthor(context));

            //Console.WriteLine(GetTotalProfitByCategory(context));

            //Console.WriteLine(GetMostRecentBooks(context));

            //IncreasePrices(context);

            //Console.WriteLine(RemoveBooks(context));





        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList());

        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            return string.Join(Environment.NewLine, context.Books
               .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
               .OrderBy(b => b.BookId)
               .Select(b => b.Title)
               .ToList());
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            return string.Join(Environment.NewLine, context.Books
                .OrderByDescending(b => b.Price)
                .Where(b => b.Price > 40)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToList()).TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToList()).TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();

            var books = new List<string>();

            foreach (var category in categories)
            {
                books.AddRange(context.BooksCategories
                   .Where(c => c.Category.Name.ToLower() == category)
                   .Select(b => b.Book.Title)
                   .ToList());

            }
            foreach (var book in books.OrderBy(b => b))
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();

            //var books = context.BooksCategories.
            //    Where(bc =>
            //          categories.Contains(bc.Category.Name.ToLower()))
            //    .Select(b => b.Book.Title)
            //    .OrderBy(b => b)
            //    .ToList();

            //return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToList()).TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            return string.Join(Environment.NewLine, context.Authors
               .Where(b => b.FirstName.EndsWith(input))
               .Select(b => $"{b.FirstName} {b.LastName}")
               .ToList()
               .OrderBy(f => f));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            return string.Join(Environment.NewLine, context.Books
               .Where(b => b.Title.ToLower().Contains(input.ToLower()))
               .OrderBy(b => b.Title)
               .Select(b => $"{b.Title}")
               .ToList()).TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            return string.Join(Environment.NewLine, context.Books
               .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
               .OrderBy(b => b.BookId)
               .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
               .ToList()).TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Count(b => b.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            return string.Join(Environment.NewLine, context.Authors
            .OrderByDescending(c => c.Books.Sum(c => c.Copies))
            .Select(a => $"{a.FirstName} {a.LastName} - {a.Books.Sum(c => c.Copies)}")
            .ToList()).TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
           var books = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    profit = c.CategoryBooks.Sum(c => c.Book.Price * c.Book.Copies)
                })
                .OrderByDescending(b => b.profit)
                .ThenBy(c => c.category)
                .ToList();

            return string.Join(Environment.NewLine, 
                books.Select(b => $"{b.category} ${b.profit:F2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                    .Select(c => new
                    {
                        categoryName = c.Name,
                        books = c.CategoryBooks
                            .OrderByDescending(b => b.Book.ReleaseDate)
                            .Select(b => new
                            {
                                title = b.Book.Title,
                                releseYear = b.Book.ReleaseDate.Value.Year

                            })
                        .Take(3)
                        .ToList()
                    })
                    .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories.OrderBy(c => c.categoryName))
            {
                sb.AppendLine($"--{category.categoryName}");
                foreach (var book in category.books)
                {
                    sb.AppendLine($"{book.title} ({book.releseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            var booksToDelete = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();
            return booksToDelete;
        }

    }

}


