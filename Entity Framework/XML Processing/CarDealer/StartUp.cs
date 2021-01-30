using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;
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
            //string inputXml = File.ReadAllText(DatasetsDirPath + "suppliers.xml");
            //string result = ImportSuppliers(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 02
            //string inputXml = File.ReadAllText(DatasetsDirPath + "parts.xml");
            //string result = ImportParts(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 03
            string inputXml = File.ReadAllText(DatasetsDirPath + "cars.xml");
            string result = ImportCars(db, inputXml);
            Console.WriteLine(result);

        }

        //TODO Problem 03
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarsDTO[]), new XmlRootAttribute("Cars"));

            ImportCarsDTO[] carsDtos = (ImportCarsDTO[]) xmlSerializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carsDtos)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                var parts = carDto
                    .Parts
                    .Where(pc => context.Parts.Any(p => p.Id == pc.Id))
                    .Select(p => p.Id)
                    .Distinct();

                foreach (var part in parts)
                {
                    PartCar partCar = new PartCar()
                    {
                        PartId = part,
                        CarId = car.Id
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);

            }

            context.Cars.AddRange(cars);

            context.PartCars.AddRange(partCars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //TODO Problem 02
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDTO[]), new XmlRootAttribute("Parts"));

            ImportPartsDTO[] partsDtos = ((ImportPartsDTO[])xmlSerializer
                .Deserialize(new StringReader(inputXml)))
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToArray();

            Part[] parts = Mapper.Map<Part[]>(partsDtos);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";

        }

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