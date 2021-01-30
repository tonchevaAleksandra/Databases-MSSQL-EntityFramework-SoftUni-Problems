using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace CarDealer
{
    public class StartUp
    {
        private const string DatasetsDirPath = @"../../../Datasets/";
        public static void Main(string[] args)
        {
            InitializeMapper();

            using CarDealerContext db = new CarDealerContext();
            //ResetDatabase(db);

            //TODO Problem 01
            string inputXml = File.ReadAllText(DatasetsDirPath + "suppliers.xml");
            string result = ImportSuppliers(db, inputXml);
            Console.WriteLine(result);

            //TODO Problem 02
            //string inputXml = File.ReadAllText("parts.xml");
            //string result = ImportParts(db, inputXml);
            //Console.WriteLine(result);
        }

        //TODO Problem 02
        //public static string ImportParts(CarDealerContext context, string inputXml)
        //{


        //}

        //TODO Problem 01
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (ImportSupplierDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var suppliers = Mapper.Map<Supplier[]>(suppliersDtos);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";


        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<CarDealerProfile>(); });
        }

        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database successfully created!");
        }
    }
}