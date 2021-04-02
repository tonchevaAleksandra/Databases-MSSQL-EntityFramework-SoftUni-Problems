using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ExportDtos
{
    [XmlType("Song")]
    public class ExportSongDto
    {
        [XmlElement]
        public string SongName { get; set; }

        [XmlElement]
        public string Writer { get; set; }

        [XmlElement]
        public string Performer { get; set; }

        [XmlElement]
        public string AlbumProducer { get; set; }

        [XmlElement]
        public string Duration { get; set; }
    }
}
