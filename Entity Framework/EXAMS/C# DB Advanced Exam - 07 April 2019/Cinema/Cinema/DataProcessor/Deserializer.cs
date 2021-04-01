using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Movie> moviesToAdd = new List<Movie>();

            ImportMovieDto[] moviesDtos = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            foreach (var movieDto in moviesDtos)
            {
                if (!IsValid(movieDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                MovieGenre genre;
                bool isValidGenre = Enum.TryParse<MovieGenre>(movieDto.Genre, out genre);

                if (!isValidGenre)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration;
                bool isValidDuration = TimeSpan.TryParse(movieDto.Duration, out duration);

                if (!isValidDuration)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (moviesToAdd.Any(x => x.Title == movieDto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Movie movie = new Movie()
                {
                    Director = movieDto.Director,
                    Duration = duration,
                    Genre = genre,
                    Rating = movieDto.Rating,
                    Title = movieDto.Title
                };

                moviesToAdd.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("f2")));
            }

            context.Movies.AddRange(moviesToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportHallDto[] hallDtos = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);
            List<Hall> hallsToAdd = new List<Hall>();

            foreach (var hallDto in hallDtos)
            {
                if (!IsValid(hallDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Hall hall = new Hall()
                {
                    Is3D = hallDto.Is3D,
                    Is4Dx = hallDto.Is4Dx,
                    Name = hallDto.Name
                };

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    hall.Seats.Add(new Seat() { Hall = hall });
                }

                string projectionType;
                if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx/3D";
                }
                else if (hall.Is3D)
                {
                    projectionType = "3D";
                }
                else if (hall.Is4Dx)
                {
                    projectionType = "4Dx";
                }
                else
                {
                    projectionType = "Normal";
                }

                hallsToAdd.Add(hall);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));
            }
            context.Halls.AddRange(hallsToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            List<Projection> projectionsToAdd = new List<Projection>();

            using (StringReader reader = new StringReader(xmlString))
            {
                ImportProjectionDto[] projectionDtos = (ImportProjectionDto[])serializer.Deserialize(reader);

                foreach (var projectionDto in projectionDtos)
                {
                    if (!IsValid(projectionDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Movie movie = context.Movies.FirstOrDefault(x => x.Id == projectionDto.MovieId);
                    if (movie == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!context.Halls.Any(x => x.Id == projectionDto.HallId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime dateTime;
                    bool isValidDate = DateTime.TryParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                    if (!isValidDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Projection projection = new Projection()
                    {
                        DateTime = dateTime,
                        HallId = projectionDto.HallId,
                        MovieId = projectionDto.MovieId
                    };

                    projectionsToAdd.Add(projection);
                    sb.AppendLine(string.Format(SuccessfulImportProjection, movie.Title,
                        projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                }
            }
                
            context.Projections.AddRange(projectionsToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();

        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            List<Customer> customersToAdd = new List<Customer>();

            using (StringReader reader= new StringReader(xmlString))
            {
                ImportCustomerDto[] customersDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);
                foreach (var customerDto in customersDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        Age = customerDto.Age,
                        Balance = customerDto.Balance,
                        FirstName = customerDto.FirstName,
                        LastName = customerDto.LastName
                    };

                    foreach (var ticketDto in customerDto.Tickets)
                    {
                        if (!IsValid(ticketDto))
                        {
                            continue;
                        }

                        Projection projection = context.Projections.FirstOrDefault(x => x.Id == ticketDto.ProjectionId);
                        if (projection==null)
                        {
                            continue;
                        }

                        Ticket ticket = new Ticket()
                        {
                            Customer = customer,
                            Price = ticketDto.Price,
                            Projection = projection
                        };

                        customer.Tickets.Add(ticket);
                    }

                    customersToAdd.Add(customer);
                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName,
                        customer.Tickets.Count));
                }

            }
            context.Customers.AddRange(customersToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}