using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using AutoMapper;
using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using CarDealer.DTO.SalesDTOs;
using CarDealer.DTO.CustomerDTOs;

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
        private static void ImportPartCars(CarDealerContext context)
        {
            int carsCount = context
                .Cars
                .Count();
            int partsCount = context.Parts.Count();

            var partCars = new List<PartCar>();

            for (int i = 1; i <= carsCount; i++)
            {
                var partCar = new PartCar();

                partCar.CarId = i;

                partCar.PartId = new Random().Next(1, partsCount);

                partCars.Add(partCar);
            }

            context.PartCars.AddRange(partCars);

            context.SaveChanges();
            Console.WriteLine($"Successfully added {partCars.Count()} partCars!");
        }
        private static void InitializeMapper()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
        }
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //ResetDatabase(db);

            //Problem 01
            //string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //string result=ImportSuppliers(db, inputJson);
            //Console.WriteLine(result);

            //Problem 02
            //string inputJson = File.ReadAllText("../../../Datasets/parts.json");
            //string result = ImportParts(db, inputJson);
            //Console.WriteLine(result);

            //Problem 03
            string inputJson = File.ReadAllText("../../../Datasets/cars.json");
            string result = ImportCars(db, inputJson);
            Console.WriteLine(result);

            //Problem 04
            //string inputJson = File.ReadAllText("../../../Datasets/customers.json");
            //string result = ImportCustomers(db, inputJson);
            //Console.WriteLine(result);

            //Problem 05
            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");
            //string result = ImportSales(db, inputJson);
            //Console.WriteLine(result);

            //Problem 06
            //EnsureDirectoryExists();
            //string json = GetOrderedCustomers(db);
            //File.WriteAllText(ResultsDirectoryPath + "/ordered-customers.json", json);

            //Problem 07
            //EnsureDirectoryExists();
            //string json = GetCarsFromMakeToyota(db);

            //File.WriteAllText(ResultsDirectoryPath + "/toyota-cars.json", json);

            //Problem 08
            //EnsureDirectoryExists();
            //string json = GetLocalSuppliers(db);
            //File.WriteAllText(ResultsDirectoryPath + "/local-suppliers.json", json);


            //--Import Parts
            //ImportPartCars(db);


            //Problem 09
            //EnsureDirectoryExists();
            //string json = GetCarsWithTheirListOfParts(db);
            //File.WriteAllText(ResultsDirectoryPath + "/cars-and-parts.json", json);

            //Problem 10
            //InitializeMapper();
            //EnsureDirectoryExists();
            //string json = GetTotalSalesByCustomer(db);
            //File.WriteAllText(ResultsDirectoryPath + "/customers-total-sales.json", json);

            //Problem 11
            EnsureDirectoryExists();
            string json = GetSalesWithAppliedDiscount(db);
            File.WriteAllText(ResultsDirectoryPath + "/sales-discounts.json", json);

        }

        //Problem 11
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new CustomerSaleDTO
                {
                    Car = new SaleCarDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                            s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100

                })
                .ToList();

            string result = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return result;
        }

        //Problem 10
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            var customers = context
                    .Customers
                    .ProjectTo<CustomerTotalSalesDTO>()
                    .Where(c => c.CarsBought >= 1)
                    .OrderByDescending(c => c.SpentMoney)
                    .ThenByDescending(c => c.CarsBought)
                    .ToList();

            string result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;

        }


        //Problem 09
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price.ToString("f2")
                    })
                    .ToList()
                })
                .ToList();

            string result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }

        //Problem 08
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            string result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }

        //Problem 07
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .ToList();

            string result = JsonConvert.SerializeObject(cars, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,

            });

            return result;
        }

        //Problem 06
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            string result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;

        }

        //Problem 05
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //Problem 04
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //Problem 03
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            List<ImportCarDto> carsDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            List<Car> cars = new List<Car>();
            foreach (var carDto in carsDtos)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (int partId in carDto.partsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Problem 02
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var suppliers = context.Suppliers.Select(s => s.Id);
            parts = parts.Where(p => suppliers.Any(s => s == p.SupplierId)).ToList();
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
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