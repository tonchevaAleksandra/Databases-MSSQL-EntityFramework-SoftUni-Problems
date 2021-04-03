using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dto.Export
{
    [XmlType("Procedure")]
  public  class ExportProcedureDto
    {
        [XmlElement]
        public string Passport { get; set; }

        [XmlElement]
        public string OwnerNumber { get; set; }

        [XmlElement]
        public string DateTime { get; set; }

        [XmlArray]
        public ExportAnimalAidDto[] AnimalAids { get; set; }

        [XmlElement]
        public decimal TotalPrice { get; set; }
    }
}
