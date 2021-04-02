using System.Globalization;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }
    }
}