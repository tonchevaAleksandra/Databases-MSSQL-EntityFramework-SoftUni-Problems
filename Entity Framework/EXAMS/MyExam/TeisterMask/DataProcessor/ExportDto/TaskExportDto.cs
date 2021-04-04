using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class TaskExportDto
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Label { get; set; }
    }
}
