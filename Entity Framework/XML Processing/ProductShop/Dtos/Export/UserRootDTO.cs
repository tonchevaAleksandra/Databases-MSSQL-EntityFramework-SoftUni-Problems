using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
   public class UserRootDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserExportDTO[] Users { get; set; }
    }
}
