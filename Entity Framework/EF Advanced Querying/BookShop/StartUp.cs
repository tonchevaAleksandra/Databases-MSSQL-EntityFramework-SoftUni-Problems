namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Proble 02
            //string input = Console.ReadLine();
            //string output = GetBooksByAgeRestriction(db, input);
            //Console.WriteLine(output);

            //Problem 03
            //Console.WriteLine(GetGoldenBooks(db));

            //Problem 04
            //Console.WriteLine(GetBooksByPrice(db));

            //Problem 05
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //Problem 06
            //string input = Console.ReadLine();

            //Console.WriteLine(GetBooksByCategory(db,input));

            //Problem 07
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //Problem 08
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //Problem 09
            //string input = Console.ReadLine().ToLower();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //Problem 10
            //string input = Console.ReadLine().ToLower();
            //Console.WriteLine(GetBooksByAuthor(db,input));

            //Problem 11
            //int n = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, n));

            //Problem 12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //Problem 13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Problem 14
            //Console.WriteLine(GetMostRecentBooks(db));

            //Problem 15
            //IncreasePrices(db);

            //Problem 16
            Console.WriteLine(RemoveBooks(db));
        }

        //Problem 16
        public static int RemoveBooks(BookShopContext context)
        {
            //var categoryBooks = context.BooksCategories
            //    .Where(bc => bc.Book.Copies < 4200);
            //context.BooksCategories.RemoveRange(categoryBooks);
            //context.SaveChanges();
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();
            int count = books.Count();
            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Count();
        }
        //Problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            var bookstoUpdate = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in bookstoUpdate)
            {
                book.Price += 5;
            }

            context.SaveChanges();

        }

        //Problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var recentBooksByCategory = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecents = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => new
                    {
                        b.Book.Title,
                        Year = b.Book.ReleaseDate.Value.Year
                    })
                })
                .OrderBy(c => c.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in recentBooksByCategory)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.MostRecents)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                    .Select(cb => new
                    {
                        BookProfit = cb.Book.Copies * cb.Book.Price
                    })
                    .Sum(c => c.BookProfit)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in categories)
            {
                sb.AppendLine($"{item.Name} ${item.TotalProfit:f2}");
            }

            return sb.ToString().Trim();

        }

        //Problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authorCopies = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    Count = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Count)
                .ToList();

            foreach (var item in authorCopies)
            {
                sb.AppendLine(item.FullName + " - " + item.Count);
            }

            return sb.ToString().Trim();
        }

        //Problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        //Problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            List<string> books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => b.Title + " (" + b.Author.FirstName + " " + b.Author.LastName + ")")
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //Problem 09
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            List<string> books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        //Problem 08
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            List<string> authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(a => a)
                .ToList();

            return string.Join(Environment.NewLine, authors);

        }

        //Problem 07
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {

            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - {item.EditionType} - ${item.Price:f2}");
            }

            return sb.ToString().Trim();

        }

        //Problem 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToList();

            List<string> bookTitles = new List<string>();

            foreach (var category in categories)
            {

                List<string> currentCategoryBooks = context.
                    Books
                    .Where(b => b.BookCategories.Any(bc => bc.Category.Name.ToLower() == category))
                    .Select(b => b.Title)
                    .ToList();
                bookTitles.AddRange(currentCategoryBooks);
            }

            return string.Join(Environment.NewLine, bookTitles.OrderBy(b => b));
        }

        //Problem 05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //Problem 04
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - ${item.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 03
        public static string GetGoldenBooks(BookShopContext context)
        {
            List<string> books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        //Problem 02
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.AgeRestriction
                     .ToString()
                     .ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();


            return String.Join(Environment.NewLine, books);
        }
    }
}
