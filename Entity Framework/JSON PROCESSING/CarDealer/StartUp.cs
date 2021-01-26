using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static string ResultsDirectoryPath = "../../../Datasets/Results";
        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(ResultsDirectoryPath))
            {
                Directory.CreateDirectory(ResultsDirectoryPath);
            }
        }
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //ResetDatabase(db);

            //Problem 01

            string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            string result=ImportSuppliers(db, inputJson);
            Console.WriteLine(result);
        }

        //Problem 01
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {

            List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";

        }

    }
}