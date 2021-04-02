using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using MusicHub.Data.Models;
using MusicHub.DataProcessor.ImportDtos;
using Newtonsoft.Json;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Writer> writersToAdd = new List<Writer>();
            ImportWriterDto[] writerDtos = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString);

            foreach (var writerDto in writerDtos)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Writer writer = new Writer()
                {
                    Name = writerDto.Name,
                    Pseudonym = writerDto.Pseudonym
                };

                writersToAdd.Add(writer);
                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }

            context.Writers.AddRange(writersToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Producer> producersToAdd = new List<Producer>();
            ImportProducerAlbumsDto[] producersDtos =
                JsonConvert.DeserializeObject<ImportProducerAlbumsDto[]>(jsonString);

            foreach (var producerDto in producersDtos)
            {
                if (!IsValid(producerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Producer producer = new Producer()
                {
                    Name = producerDto.Name,
                    Pseudonym = producerDto.Pseudonym,
                    PhoneNumber = producerDto.PhoneNumber
                };
                bool isAllAlbumsValid = true;
                foreach (var albumDto in producerDto.Albums)
                {
                    if (!IsValid(albumDto))
                    {
                        isAllAlbumsValid = false;
                        break;
                    }

                    DateTime releaseDate;
                    bool isValidDate = DateTime.TryParseExact(albumDto.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);
                    if (!isValidDate)
                    {
                        isAllAlbumsValid = false;
                        break;
                    }

                    Album album = new Album()
                    {
                        Producer = producer,
                        Name = albumDto.Name,
                        ReleaseDate = releaseDate
                    };

                    producer.Albums.Add(album);
                }

                if (!isAllAlbumsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                producersToAdd.Add(producer);
                string message = producer.PhoneNumber != null
                    ? string.Format(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber,
                        producer.Albums.Count)
                    : string.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count);
                sb.AppendLine(message);
            }

            context.Producers.AddRange(producersToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
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