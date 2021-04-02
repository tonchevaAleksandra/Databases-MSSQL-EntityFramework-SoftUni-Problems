using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string DatasetsDirPath = @"../../../Datasets/";
        private const string ResultDirPath = DatasetsDirPath + "Results/";
        public static void Main(string[] args)
        {
            //InitializeMapper();

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
            //string inputXml = File.ReadAllText(DatasetsDirPath + "cars.xml");
            //string result = ImportCars(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 04
            //string inputXml = File.ReadAllText(DatasetsDirPath + "customers.xml");
            //string result = ImportCustomers(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 05
            //string inputXml = File.ReadAllText(DatasetsDirPath + "sales.xml");
            //string result = ImportSales(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 06
            //string result = GetCarsWithDistance(db);
            //File.WriteAllText(ResultDirPath + "cars.xml", result);

            //TODO Problem 07
            //string result = GetCarsFromMakeBmw(db);
            //File.WriteAllText(ResultDirPath + "bmw-cars.xml", result);

            //TODO Problem 08
            //string result = GetLocalSuppliers(db);
            //File.WriteAllText(ResultDirPath + "local-suppliers.xml", result);

            //TODO Problem 09
            string result = GetCarsWithTheirListOfParts(db);
            File.WriteAllText(ResultDirPath + "cars-and-parts.xml", result);

            //TODO Problem 10
            //string result = GetTotalSalesByCustomer(db);
            //File.WriteAllText(ResultDirPath + "customers-total-sales.xml", result);

            //TODO Problem 11
            //string result = GetSalesWithAppliedDiscount(db);
            //File.WriteAllText(ResultDirPath + "sales-discounts.xml", result);

        }

        //TODO Problem 11
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var sales = context
                .Sales
                .Select(x=> new ExportSaleDTO()
                {
                    Car= new ExportCarSaleDTO()
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    CustomerName = x.Customer.Name,
                    Discount = x.Discount,
                    Price = x.Car.PartCars.Sum(c=>c.Part.Price),
                    PriceWithDiscount = x.Car.PartCars.Sum(c => c.Part.Price)-
                                        x.Car.PartCars.Sum(c => c.Part.Price)*x.Discount/100
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSaleDTO[]), new XmlRootAttribute("sales"));

            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 10
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Select(x=> new ExportCustomerTotalSalesDTO()
                    {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(s=>s.Car.PartCars.Sum(p=>p.Part.Price))
                    })
                .Where(c => c.BoughtCars >= 1)
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportCustomerTotalSalesDTO[]), new XmlRootAttribute("customers"));

            xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 09
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            var cars = context
                .Cars
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .Select(x=> new ExportCarWithPartsDTO()
                {
                    Make = x.Make,
                    Model = x.Model,
                    Parts = x.PartCars.Select(y=>new ExportPartCarsDTO()
                    {
                        Name = y.Part.Name,
                        Price = y.Part.Price
                    }).OrderByDescending(y => y.Price)
                        .ToArray(),
                    TravelledDistance = x.TravelledDistance

                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportCarWithPartsDTO[]), new XmlRootAttribute("cars"));

            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();


        }

        //TODO Problem 08
        public static string GetLocalSuppliers(CarDealerContext context)
        {

            StringBuilder sb = new StringBuilder();

            var suppliers = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .Select(x=> new ExportLocalSuppliersDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportLocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 07
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(x=> new ExportCarsBMWDTO()
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsBMWDTO[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }


        //TODO Problem 06
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(x=> new ExportCarWithDistanceDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportCarWithDistanceDTO[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); ;
            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 05
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));

            ImportSaleDTO[] salesDtos = (ImportSaleDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            //var sales = Mapper.Map<Sale[]>(salesDtos).Where(s => context.Cars.Any(c => c.Id == s.CarId)).ToArray();

            List<Sale> sales = new List<Sale>();
            foreach (var saleDto in salesDtos)
            {
                if (/*context.Customers.Any(c => c.Id == saleDto.CustomerId) &&*/
                    context.Cars.Any(c=>c.Id==saleDto.CarId))
                {
                    Sale sale = new Sale()
                    {
                        CarId = saleDto.CarId,
                        CustomerId = saleDto.CustomerId,
                        Discount = saleDto.Discount
                    };

                    sales.Add(sale);
                }
               
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
            ;
        }

        //TODO Problem 04
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));

            ImportCustomerDTO[] customersDtos =
                (ImportCustomerDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            //var customers = Mapper.Map<Customer[]>(customersDtos);

            List<Customer> customers = new List<Customer>();
            foreach (var customerDto in customersDtos)
            {

                DateTime date;
                bool isValidDate = DateTime.TryParseExact(customerDto.BirthDate, "yyyy-MM-dd'T'HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                if (isValidDate)
                {
                    Customer customer = new Customer()
                    {
                        Name = customerDto.Name,
                        BirthDate = date,
                        IsYoungDriver = customerDto.IsYoungDriver
                    };
                    customers.Add(customer);
                }
               
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";

        }

        //TODO Problem 03
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarsDTO[]), new XmlRootAttribute("Cars"));

            ImportCarsDTO[] carsDtos = (ImportCarsDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

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
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);

            }

            context.PartCars.AddRange(partCars);

            context.Cars.AddRange(cars);

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

            //Part[] parts = Mapper.Map<Part[]>(partsDtos);
            List<Part> parts = new List<Part>();
            foreach (var partsDto in partsDtos)
            {
                if (context.Suppliers.Any(c => c.Id == partsDto.SupplierId))
                {
                    Part part = new Part()
                    {
                        Name = partsDto.Name,
                        Price = partsDto.Price,
                        Quantity = partsDto.Quantity,
                        SupplierId = partsDto.SupplierId
                    };

                    parts.Add(part);
                }
               
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";

        }

        //TODO Problem 01
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (ImportSupplierDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            //var suppliers = Mapper.Map<Supplier[]>(suppliersDtos);

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var importSupplierDto in suppliersDtos)
            {
                Supplier supplier = new Supplier()
                {
                    Name = importSupplierDto.Name,
                    IsImporter = importSupplierDto.IsImporter
                };
                suppliers.Add(supplier);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";


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