using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using MyCoolCarSystem.Data;
using MyCoolCarSystem.Data.Models;
using MyCoolCarSystem.Data.Results;

namespace MyCoolCarSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using var db = new CarDbContext();
            db.Database.Migrate();

            //AddMakes(db);

            //AddModelsToOpelMake(db);

            //AddCarsToInsigniaModel(db);

            //AddCustomerAndPurchase(db);

            //AddAddressToCustomer(db);

            //QueringCarsOwnerByResultModels(db);

            //ValidateEntity(db);

            //SetValueToSecretPropertyInCar(db);


            db.SaveChanges();
        }

        private static void SetValueToSecretPropertyInCar(CarDbContext db)
        {
            var car = db.Cars.FirstOrDefault();
            db.Entry(car).Property<int>("MySecretProperty").CurrentValue = 15;
        }

        private static void ValidateEntity(CarDbContext db)
        {
            var make = db.Makes.FirstOrDefault(m => m.Name == "Mercedes");
            var model = new Model
            {
                Modification = "500",
                Name = "CL",
                Year = 2018
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            Validator.ValidateObject(model, validationContext, true);

            Validator.TryValidateObject(model, validationContext, validationResults, true);
            make.Models.Add(model);
        }

        private static void QueringCarsOwnerByResultModels(CarDbContext db)
        {
            db.CarsOwners
                .Select(p => new PurchaseResultModel()
                {
                    Price = p.Price,
                    PurchaseDate = p.PurchaseDate,
                    Customer = new CustomerResultModel()
                    {
                        Name = p.Customer.FirstName + " " + p.Customer.LastName,
                        Town = p.Customer.Address.Town,
                    },
                    Car = new CarResultModel()
                    {
                        Make = p.Car.Model.Make.Name,
                        Model = p.Car.Model.Name,
                        Vin = p.Car.Vin

                    }
                })
                .ToList();
        }

        private static void AddAddressToCustomer(CarDbContext db)
        {
            var customer = db.Customers.FirstOrDefault();

            customer.Address = new Address
            {
                Text = "Tintyava 15",
                Town = "Sofia"
            };
        }

        private static void AddCustomerAndPurchase(CarDbContext db)
        {
            var car = db.Cars.FirstOrDefault();
            var customer = new Customer
            {
                FirstName = "Ivan",
                LastName = "Petrov",
                Age = 29
            };

            customer.Purchases.Add(new CarPurchase
            {
                Car = car,
                PurchaseDate = DateTime.UtcNow.AddDays(-15),
                Price = car.Price * 0.9M,

            });
            db.Customers.Add(customer);
        }

        private static void AddCarsToInsigniaModel(CarDbContext db)
        {
            var insigniaModel = db.Models.FirstOrDefault(m => m.Name == "Insignia");
            insigniaModel.Cars.Add(new Car
            {
                Color = "Black",
                Price = 20000,
                ProductionDate = DateTime.Now.AddDays(-100),
                Vin = "HILD5F48FKBIUGBJ"

            });
            insigniaModel.Cars.Add(new Car
            {
                Color = "Dark Blue",
                Price = 21000,
                ProductionDate = DateTime.Now.AddDays(-200),
                Vin = "HGOIGFLIFKNLKGGIF"

            });
            insigniaModel.Cars.Add(new Car
            {
                Color = "Graffitty Grey",
                Price = 25000,
                ProductionDate = DateTime.Now.AddDays(-300),
                Vin = "VIRHGE94HGIRDFRE"

            });
        }

        private static void AddModelsToOpelMake(CarDbContext db)
        {
            var opelMake = db.Makes.FirstOrDefault(m => m.Name == "Opel");
            opelMake.Models.Add(new Model
            {
                Name = "Astra",
                Year = 2017,
                Modification = "OPC"
            });

            opelMake.Models.Add(new Model
            {
                Name = "Insignia",
                Year = 2019,
                Modification = "2.2 TDI"
            });
        }

        private static void AddMakes(CarDbContext db)
        {
            db.Makes.Add(new Make
            {
                Name = "Mercedes"
            });
            db.Makes.Add(new Make
            {
                Name = "BMW"
            });
            db.Makes.Add(new Make
            {
                Name = "Audi"
            });
            db.Makes.Add(new Make
            {
                Name = "Opel"
            });
            db.Makes.Add(new Make
            {
                Name = "Peugeot"
            });
        }
    }
}
