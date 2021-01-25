using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {

        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);

            string inputJson = File.ReadAllText("../../../Datasets/users.json");

            var result = ImportUsers(db, inputJson);
            Console.WriteLine(result);
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            int count = users.Length;
            context.SaveChanges();

            return $"Successfully imported {count}";
        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
    }
}