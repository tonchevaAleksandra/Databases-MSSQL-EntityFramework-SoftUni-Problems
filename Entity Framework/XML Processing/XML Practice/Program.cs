using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XML_Practice
{
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


            XDocument xdocument = XDocument.Load("bgwiki-20210120-abstract.xml");
            var articles = xdocument.Root.Elements()
                .Where(x => x.Element("title").Value.Contains("Николай"))
                .OrderBy(x => x.Element("title").Value)
                .Select(x => new
                {
                    Title = x.Element("title").Value,
                    Description = x.Element("abstract").Value,
                    Url = x.Element("url").Value
                });

            foreach (var article in articles)
            {
                Console.WriteLine(article.Title);
            }
        }
    }

    class Plane
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }
}
