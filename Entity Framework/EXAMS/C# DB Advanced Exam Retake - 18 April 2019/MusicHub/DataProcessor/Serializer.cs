using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MusicHub.DataProcessor.ExportDtos;
using Newtonsoft.Json;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums.Where(x => x.Producer.Id == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.Select(y => new
                    {
                        SongName = y.Name,
                        Price = y.Price.ToString("f2"),
                        Writer = y.Writer.Name
                    })
                        .OrderByDescending(y => y.SongName)
                        .ThenBy(y => y.Writer)
                        .ToList(),
                    AlbumPrice = x.Songs.Sum(y => y.Price).ToString("f2")
                })
                .OrderByDescending(x => x.AlbumPrice)
                .ToList();

            string json = JsonConvert.SerializeObject(albums, Formatting.Indented);

            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            var encoding = Encoding.UTF8;
            var songs = context.Songs.Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new ExportSongDto()
                {
                    SongName = x.Name,
                    Writer = x.Writer.Name,
                    Performer = x.SongPerformers.FirstOrDefault().Performer.FirstName + " " +
                                x.SongPerformers.FirstOrDefault().Performer.LastName,
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c")
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.Writer)
                .ThenBy(x => x.Performer)
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportSongDto[]), new XmlRootAttribute("Songs"));
            
            xmlSerializer.Serialize(new StringWriter(sb), songs, namespaces);

            return sb.ToString().Trim();
        }
    }
}