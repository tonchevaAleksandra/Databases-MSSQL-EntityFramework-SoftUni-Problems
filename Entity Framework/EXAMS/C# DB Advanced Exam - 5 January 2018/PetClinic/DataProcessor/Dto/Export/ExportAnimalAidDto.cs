using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dto.Export
{
    [XmlType("AnimalAid")]
    public class ExportAnimalAidDto
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public decimal Price { get; set; }
    }
}
