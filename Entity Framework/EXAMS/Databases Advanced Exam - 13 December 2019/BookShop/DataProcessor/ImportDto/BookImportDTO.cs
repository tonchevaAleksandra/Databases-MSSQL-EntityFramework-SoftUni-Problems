namespace BookShop.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    [XmlType("Book")]
    public class BookImportDTO
    {

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Range(1, 3)]
        [XmlElement("Genre")]
        public int Genre { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [Range(50, 5000)]
        [XmlElement("Pages")]
        public int Pages { get; set; }

        [XmlElement("PublishedOn")]
        public string PublishedOn { get; set; }
    }
}
