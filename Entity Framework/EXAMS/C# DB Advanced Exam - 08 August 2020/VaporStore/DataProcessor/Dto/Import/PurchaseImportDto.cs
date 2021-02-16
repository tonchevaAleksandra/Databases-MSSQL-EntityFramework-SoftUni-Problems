namespace VaporStore.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [Required]
        [XmlElement("Type")]
        public string PurchaseType { get; set; }

        [Required]
        [XmlAttribute("title")]
        public string Game { get; set; }

        [Required]
        [XmlElement("Key")]
        [RegularExpression(@"^[A-Z,0-9]{4}-[A-Z,0-9]{4}-[A-Z,0-9]{4}$")]
        public string ProductKey { get; set; }

        [Required]
        [XmlElement("Card")]
        [RegularExpression(@"^(\d){4} (\d){4} (\d){4} (\d){4}$")]
        public string CardNumber { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }

    }
}
