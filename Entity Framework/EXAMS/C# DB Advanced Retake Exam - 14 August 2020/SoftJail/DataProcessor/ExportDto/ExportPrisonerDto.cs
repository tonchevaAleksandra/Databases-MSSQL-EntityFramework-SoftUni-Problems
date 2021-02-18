using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
 public  class ExportPrisonerDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray]
        public ExportMailDto[] EncryptedMessages { get; set; }
    }
}
