using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto.Import;

namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;

    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyAddedGame = "Added {0} ({1}) with {2} tags";
        private const string SuccessfullyAddedUser = "Imported {0} with {1} cards";
        private const string SuccessfullyAddedPurchase = "Imported {0} for {1}";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<Game> gamestoAdd = new List<Game>();

            GameImportDto[] games = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString);
            List<Developer> developers = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();
            foreach (var game in games)
            {
                if (!IsValid(game))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                DateTime releaseDate;
                bool isDateValid = DateTime.TryParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out releaseDate);
                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (game.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game gametoAdd = new Game()
                {
                    Name = game.Name,
                    Price = game.Price,
                    ReleaseDate = releaseDate
                };
                var dev = developers.FirstOrDefault(d => d.Name == game.Developer);
                if (dev == null)
                {
                    dev = new Developer()
                    {
                        Name = game.Developer
                    };

                    developers.Add(dev);
                }

                gametoAdd.Developer = dev;

                var genre = genres.FirstOrDefault(g => g.Name == game.Genre);
                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = game.Genre
                    };
                    genres.Add(genre);
                }

                gametoAdd.Genre = genre;

                foreach (var tagItem in game.Tags)
                {
                    if (String.IsNullOrEmpty(tagItem))
                    {
                        continue;
                    }

                    Tag tag = tags.FirstOrDefault(x => x.Name == tagItem);
                    if (tag == null)
                    {
                        Tag t = new Tag()
                        {
                            Name = tagItem
                        };
                        tags.Add(t);

                        gametoAdd.GameTags.Add(new GameTag()
                        {
                            Game = gametoAdd,
                            Tag = t
                        });
                    }
                    else
                    {
                        gametoAdd.GameTags.Add(new GameTag()
                        {
                            Game = gametoAdd,
                            Tag = tag
                        });
                    }

                }

                if (!gametoAdd.GameTags.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                gamestoAdd.Add(gametoAdd);
                sb.AppendLine(string.Format(SuccessfullyAddedGame, gametoAdd.Name, gametoAdd.Genre.Name,
                    gametoAdd.GameTags.Count));
            }

            context.Games.AddRange(gamestoAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<User> usersToAdd = new List<User>();

            UserImportDto[] usersDtos = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString);


            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (userDto.Cards.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    Age = userDto.Age,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    Username = userDto.Username
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    CardType type;
                    bool isValidCardType = Enum.TryParse(cardDto.Type, out type);
                    if (!isValidCardType)
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    Card card = new Card()
                    {
                        Cvc = cardDto.Cvc,
                        Number = cardDto.Number,
                        Type = type,
                        User = user
                    };
                    user.Cards.Add(card);
                    //context.Cards.Add(card);
                    //context.SaveChanges();
                }

                usersToAdd.Add(user);
                sb.AppendLine(string.Format(SuccessfullyAddedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(usersToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();

        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));

            using (StringReader reader = new StringReader(xmlString))
            {
                PurchaseImportDto[] purchasesDtos = (PurchaseImportDto[])xmlSerializer.Deserialize(reader);

                List<Purchase> purchaseToAdd = new List<Purchase>();

                foreach (var purchasesDto in purchasesDtos)
                {
                    if (!IsValid(purchasesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime date;
                    bool isDateValid = DateTime.TryParseExact(purchasesDto.Date, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                    if (!isDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PurchaseType type;
                    bool isValidType = Enum.TryParse(purchasesDto.PurchaseType, out type);
                    if (!isValidType)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = context.Cards.FirstOrDefault(c => c.Number == purchasesDto.CardNumber);
                    if (card == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Game game = context.Games.FirstOrDefault(g => g.Name == purchasesDto.Game);
                    if (game == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Purchase purchase = new Purchase()
                    {
                        Card = card,
                        Date = date,
                        Game = game,
                        ProductKey = purchasesDto.ProductKey,
                        Type = type
                    };

                    purchaseToAdd.Add(purchase);
                    sb.AppendLine(string.Format(SuccessfullyAddedPurchase, purchase.Game.Name,
                        purchase.Card.User.Username));
                }

                context.Purchases.AddRange(purchaseToAdd);
                context.SaveChanges();

                return sb.ToString().Trim();
            }
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}