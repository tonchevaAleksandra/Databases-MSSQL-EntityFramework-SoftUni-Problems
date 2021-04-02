using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Song")]
    public class ImportPerformerSongDto
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }
    }
}
