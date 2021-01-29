using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XML_Practice
{
    [XmlType("doc")]
    public class Article
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("abstract")]
        public string Description { get; set; }

        [XmlIgnore]
        public string url { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //var xml = File.ReadAllText("Planes.xml");
            ////XDocument xmlDocument = XDocument.Parse(xml);
            //XDocument xmlDocument = XDocument.Load("Planes.xml");

            //Console.WriteLine(xmlDocument.Declaration.Encoding);
            //Console.WriteLine(xmlDocument.Root.Elements().Count());
            //Console.WriteLine(xmlDocument.Root.Elements()
            //    .FirstOrDefault(x=>x.Element("color").Value=="Blue").Element("year").Value);

            //Console.OutputEncoding = Encoding.UTF8;



            //foreach (var article in xdocument.Root.Elements())
            //{
            //    article.SetAttributeValue("lang", "bg");
            //    article.SetElementValue("links",null);
            //    //article.Element("title").SetAttributeValue("lang", "bg");
            //}
            //xdocument.Save("bgwiki_updated.xml");
            ;
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(doc[]), new XmlRootAttribute("feed"));
            //var docs = (doc[])xmlSerializer.Deserialize(File.OpenRead("bgwiki_updated.xml"));

            //XDocument xdocument = XDocument.Load("bgwiki_updated.xml");
            //var articles = docs
            //    .Where(x => x.title.Contains("Николай"))
            //    .OrderBy(x => x.title);

            //foreach (var article in articles)
            //{
            //    Console.WriteLine(article.title);
            //}

            List<Article> docs = new List<Article>
            {
                new Article {Title = "България", Description = "държава...", url = "http://wikipedia"},
                new Article {Title = "Софтуни", Description = "Образователен център...", url = "http://wikipedia"},
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            xmlSerializer.Serialize(File.OpenWrite("myDocs.xml"), docs);
        }
    }

    //class Plane
    //{
    //    public int Year { get; set; }
    //    public string Make { get; set; }
    //    public string Model { get; set; }
    //    public string Color { get; set; }
    //}
}
