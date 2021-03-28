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
                    bool isValidDate = DateTime.TryParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedOn);

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

            context.Books.AddRange(booksToAdd);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Author> authorsToAdd = new List<Author>();
            AuthorDto[] authorsDtos = JsonConvert.DeserializeObject<AuthorDto[]>(jsonString);

            foreach (var authorsDto in authorsDtos)
            {
                if (!IsValid(authorsDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authorsToAdd.Any(x => x.Email == authorsDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author()
                {
                    FirstName = authorsDto.FirstName,
                    LastName = authorsDto.LastName,
                    Phone = authorsDto.Phone,
                    Email = authorsDto.Email
                };

                foreach (var authorBookDto in authorsDto.Books)
                {
                    if (!authorBookDto.BookId.HasValue)
                    {
                        continue;
                    }

                    Book book = context.Books.FirstOrDefault(x => x.Id == authorBookDto.BookId);

                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook()
                    {
                        Book = book,
                        Author = author
                    });
                }

                if (!author.AuthorsBooks.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authorsToAdd.Add(author);
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName,
                    author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authorsToAdd);
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