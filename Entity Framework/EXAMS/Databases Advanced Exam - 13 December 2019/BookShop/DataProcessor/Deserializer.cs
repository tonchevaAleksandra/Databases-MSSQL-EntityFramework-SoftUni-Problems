namespace BookShop.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;

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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookImportDTO[]), new XmlRootAttribute("Books"));

            using (StringReader stringReader = new StringReader(xmlString))
            {
                BookImportDTO[] booksDTOs = (BookImportDTO[])xmlSerializer.Deserialize(stringReader);

                List<Book> booksToAdd = new List<Book>();

                foreach (var bookDtO in booksDTOs)
                {
                    if (!IsValid(bookDtO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime publishedOn;

                    bool isDateValid = DateTime.TryParseExact(bookDtO.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedOn);

                    if (!isDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Book book = new Book()
                    {
                        Name = bookDtO.Name,
                        Genre = (Genre)bookDtO.Genre,
                        Price = bookDtO.Price,
                        Pages = bookDtO.Pages,
                        PublishedOn = publishedOn
                    };

                    booksToAdd.Add(book);

                    sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
                }

                context.Books.AddRange(booksToAdd);

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Author> authorsToAdd = new List<Author>();
            ImportAuthorDTO[] authorDtos = JsonConvert.DeserializeObject<ImportAuthorDTO[]>(jsonString);

            foreach (var authorDto in authorDtos)
            {
                if (!IsValid(authorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authorsToAdd.Any(a => a.Email == authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email

                };

                foreach (var bookDto in authorDto.Books)
                {
                    if (!bookDto.BookId.HasValue)
                    {
                        continue;
                    }
                    Book book = context.Books.FirstOrDefault(x => x.Id == bookDto.BookId);

                    if (book == null)
                    {
                        continue;
                    }
                    author.AuthorsBooks.Add(new AuthorBook()
                    {
                        Author = author,
                        Book = book
                    });
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authorsToAdd.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName,
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