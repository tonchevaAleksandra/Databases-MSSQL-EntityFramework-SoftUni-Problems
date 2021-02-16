using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using VaporStore.Data.Models;
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

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<Game> gamestoAdd = new List<Game>();

            GameImportDto[] games = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString);

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

                List<Tag> tagsToAdd = new List<Tag>();
                foreach (var tag in game.Tags)
                {
                    if (String.IsNullOrEmpty(tag))
                    {
                        continue;
                    }

                    Tag currenTag = context.Tags.FirstOrDefault(x => x.Name == tag);
                    if (currenTag == null)
                    {
                        currenTag = new Tag()
                        {
                            Name = tag
                        };
                        context.Tags.Add(currenTag);
                        context.SaveChanges();
                    }
                    
                    if (tagsToAdd.Contains(currenTag))
                    {
                        continue;
                    }
                    
                    tagsToAdd.Add(currenTag);

                }

                if (!tagsToAdd.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
               

                var dev = context.Developers.FirstOrDefault(d => d.Name == game.Developer);
                if (dev == null)
                {
                    dev = new Developer()
                    {
                        Name = game.Name
                    };

                    context.Developers.Add(dev);
                    context.SaveChanges();
                }

                var genre = context.Genres.FirstOrDefault(g => g.Name == game.Genre);
                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = game.Genre
                    };
                    context.Genres.Add(genre);
                    context.SaveChanges();
                }

                Game gametoAdd = new Game()
                {
                    Developer = dev,
                    Genre = genre,
                    Name = game.Name,
                    Price = game.Price,
                    ReleaseDate = releaseDate
                };

                foreach (var tag in tagsToAdd)
                {
                    GameTag gt = new GameTag()
                    {
                        Tag = tag,
                        Game = gametoAdd
                    };
                    gametoAdd.GameTags.Add(gt);
                    //context.GameTags.Add(gt);
                    //context.SaveChanges();
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
            throw new NotImplementedException();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
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