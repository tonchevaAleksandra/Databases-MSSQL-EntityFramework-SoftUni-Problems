using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;
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
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSongDto[]), new XmlRootAttribute("Songs"));
            List<Song> songsToAdd = new List<Song>();

            using (StringReader reader= new StringReader(xmlString))
            {
                ImportSongDto[] songsDtos = (ImportSongDto[]) xmlSerializer.Deserialize(reader);

                foreach (var songDto in songsDtos)
                {
                    if (!IsValid(songDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TimeSpan duration;
                    bool isValidDuration = TimeSpan.TryParseExact(songDto.Duration, "c", CultureInfo.InvariantCulture,
                        TimeSpanStyles.None, out duration);
                    if (!isValidDuration)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime createdOn;
                    bool isValidDate = DateTime.TryParseExact(songDto.CreatedOn, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out createdOn);
                    if (!isValidDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Genre genre;
                    bool isValidGenre = Enum.TryParse<Genre>(songDto.Genre, out genre);
                    if (!isValidGenre)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Album album = context.Albums.FirstOrDefault(x => x.Id == songDto.AlbumId);
                    if (album==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Writer writer = context.Writers.FirstOrDefault(x => x.Id == songDto.WriterId);
                    if (writer==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Song song = new Song()
                    {
                        Album = album,
                        CreatedOn = createdOn,
                        Duration = duration,
                        Genre = genre,
                        Name = songDto.Name,
                        Price = songDto.Price,
                        Writer = writer
                    };

                    songsToAdd.Add(song);
                    sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre.ToString(),
                        song.Duration.ToString("c")));
                }
            }

            context.Songs.AddRange(songsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportPerformerDto[]), new XmlRootAttribute("Performers"));
            List<Performer> performersToAdd = new List<Performer>();

            using (StringReader reader= new StringReader(xmlString))
            {
                ImportPerformerDto[] performerDtos = (ImportPerformerDto[]) xmlSerializer.Deserialize(reader);

                foreach (var performerDto in performerDtos)
                {
                    if (!IsValid(performerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Performer performer = new Performer()
                    {
                        FirstName = performerDto.FirstName,
                        LastName = performerDto.LastName,
                        Age = performerDto.Age,
                        NetWorth = performerDto.NetWorth
                    };

                    bool isAllSongsValid = true;

                    foreach (var songDto in performerDto.PerformersSongs)
                    {
                        Song song = context.Songs.FirstOrDefault(x => x.Id == songDto.Id);
                        if (song==null)
                        {
                            isAllSongsValid = false;
                            break;
                        }

                        performer.PerformerSongs.Add(new SongPerformer()
                        {
                            Performer = performer,
                            Song = song
                        });
                    }

                    if (!isAllSongsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    performersToAdd.Add(performer);
                    sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName,
                        performer.PerformerSongs.Count));
                }
            }

            context.Performers.AddRange(performersToAdd);
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