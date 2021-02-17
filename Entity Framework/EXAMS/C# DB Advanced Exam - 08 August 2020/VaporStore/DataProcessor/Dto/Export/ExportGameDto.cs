namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class ExportGameDto
    {
        [XmlAttribute("title")]
        public string Name { get; set; }
        [XmlElement("Genre")]
        public string GenreName { get; set; }
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
