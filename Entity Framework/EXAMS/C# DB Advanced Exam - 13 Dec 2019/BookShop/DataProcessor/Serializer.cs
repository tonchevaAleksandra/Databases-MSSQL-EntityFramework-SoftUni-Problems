using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ExportDto;

namespace BookShop.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var craziestAuthors = context.Authors
                .Select(x => new
                {
                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks
                        .OrderByDescending(y=>y.Book.Price)
                        .Select(y => new
                    {
                        BookName = y.Book.Name,
                        BookPrice = y.Book.Price.ToString("f2")
                    })
                        .ToList()
                       
                })
                .ToList()
                .OrderByDescending(x => x.Books.Count())
                .ThenBy(x => x.AuthorName);

            string jsonResult = JsonConvert.SerializeObject(craziestAuthors, Formatting.Indented);

            return jsonResult;

        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            List<ExportBookDto> booksDtos = context.Books
                .Where(x => x.PublishedOn < date && x.Genre == Genre.Science)
                .ToList()
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Take(10)
                .Select(x => new ExportBookDto()
                {
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                    Name = x.Name,
                    Pages = x.Pages
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExportBookDto>), new XmlRootAttribute("Books"));
            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, booksDtos, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}