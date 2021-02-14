using System.Collections.Generic;
using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ExportDto;

namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {

            var authors = context.Authors.Select(a => new
            {
                AuthorName = a.FirstName + " " + a.LastName,
                Books = a.AuthorsBooks
                        .OrderByDescending(b => b.Book.Price)
                        .Select(b => new
                        {
                            BookName = b.Book.Name,
                            BookPrice = $"{b.Book.Price:F2}"
                        })
                        .ToList()

            })
                .ToList()
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName);

            var jsonResult = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {

            List<BookExportDTO> books = context.Books
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .ToList()
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .Take(10)
                .Select(b => new BookExportDTO()
                {
                    Date = b.PublishedOn.ToString("d",CultureInfo.InvariantCulture),
                    Name = b.Name,
                    Pages = b.Pages
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BookExportDTO>), new XmlRootAttribute("Books"));
            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, books, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}