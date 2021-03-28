using BookShop.Data.Models;
using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ImportDto;

namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(BookDto[]), new XmlRootAttribute("Books"));
            List<Book> booksToAdd = new List<Book>();
            using (StringReader reader = new StringReader(xmlString))
            {
                var bookDtos = (BookDto[])serializer.Deserialize(reader);

                foreach (var bookDto in bookDtos)
                {
                    if (!IsValid(bookDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime publishedOn;
                    bool isValidDate = DateTime.TryParseExact(bookDto.PublisheOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedOn);

                    if (!isValidDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Book book = new Book()
                    {
                        Genre = (Genre)bookDto.Genre,
                        Name = bookDto.Name,
                        Pages = bookDto.Pages,
                        Price = bookDto.Price,
                        PublishedOn = publishedOn
                    };

                    booksToAdd.Add(book);

                    sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
                }
            }

            context.AddRange(booksToAdd);
            context.SaveChanges();
            return sb.ToString();

        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}