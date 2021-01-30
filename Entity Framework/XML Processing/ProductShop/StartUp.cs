using System;
using System.IO;
using System.Xml.Serialization;
using ProductShop.Data;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);

            //TODO  Problem 01 
            var inputXml = File.ReadAllText("users.xml");
            var result = ImportUsers(db, inputXml);


        }

        //TODO  Problem 01 
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            return " ";
        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database is successfully deleted");
            db.Database.EnsureCreated();
            Console.WriteLine("Database is successfully created");
        }
    }
}