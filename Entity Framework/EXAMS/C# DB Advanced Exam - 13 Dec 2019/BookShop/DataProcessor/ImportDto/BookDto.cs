using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class BookDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Range(1,3)]
        public int Genre { get; set; }

        [Range(typeof(decimal),"0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(50, 500)]
        public int Pages { get; set; }

        [Required]
        public string PublisheOn { get; set; }
    }
}
