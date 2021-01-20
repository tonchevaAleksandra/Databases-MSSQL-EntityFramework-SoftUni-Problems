namespace BookShop
{
    using BookShop.Models;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();
            string output = GetBooksByAgeRestriction(db, input);
            Console.WriteLine(output);

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
