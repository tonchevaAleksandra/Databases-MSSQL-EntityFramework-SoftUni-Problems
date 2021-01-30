using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text.Json;

namespace XML_Practice
{
    [XmlType("doc")]
    [Serializable]
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            var docs = (List<Article>)xmlSerializer.Deserialize(File.OpenRead("bgwiki_updated.xml"));

            //XDocument xdocument = XDocument.Load("bgwiki_updated.xml");
            //var articles = docs
            //    .Where(x => x.Title.Contains("Николай"))
            //    .OrderBy(x => x.Title);

            //foreach (var article in articles)
            //{
            //    Console.WriteLine(article.Title);
            //    Console.WriteLine("     " + article.Description);
            //    Console.WriteLine(new string('-', 60));
            //}

            //xmlSerializer.Serialize(File.OpenWrite("test.xml"), docs);

            //using (var xmlWriter = XmlWriter.Create(File.OpenWrite("test_unindented.xml"),
            //    new XmlWriterSettings { Indent = true }))
            //{
            //    xmlSerializer.Serialize(xmlWriter, docs);
            //}


            //File.WriteAllText("test.json", JsonSerializer.Serialize(docs, new JsonSerializerOptions { WriteIndented = true })); ;
            //File.WriteAllText("test_unindented.json", JsonSerializer.Serialize(docs));

            var binarySerializer = new BinaryFormatter();
            //binarySerializer.Serialize(File.OpenWrite("test.bin"), docs);
            var articles= binarySerializer.Deserialize(File.OpenRead("test.bin")) as List<Article>;


            //List<Article> docs = new List<Article>
            //{
            //    new Article {Title = "България", Description = "държава...", url = "http://wikipedia"},
            //    new Article {Title = "Софтуни", Description = "Образователен център...", url = "http://wikipedia"},
            //};

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            //xmlSerializer.Serialize(File.OpenWrite("myDocs.xml"), docs);


        }
    }

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class breakfast_menu
    {

        private breakfast_menuFood[] foodField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("food")]
        public breakfast_menuFood[] food
        {
            get
            {
                return this.foodField;
            }
            set
            {
                this.foodField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class breakfast_menuFood
    {

        private string nameField;

        private string priceField;

        private string descriptionField;

        private ushort caloriesField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public ushort calories
        {
            get
            {
                return this.caloriesField;
            }
            set
            {
                this.caloriesField = value;
            }
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
