using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using MyCoolCarSystem.Data;
using MyCoolCarSystem.Data.Models;
using MyCoolCarSystem.Data.Queries;
using MyCoolCarSystem.Results;


namespace MyCoolCarSystem
{
    public class StartUp
    {
        public static void Main()
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

            var price = 5000;
            db.Cars
                .Where(c => c.Price > price)
                .ToList();

            db.Cars
                .FromSqlRaw("SELECT * FROM  Cars WHERE Price> {0}", price)
                .ToList();

            db.Cars
                .FromSqlInterpolated($"SELECT * FROM  Cars WHERE Price> {price}")
                .ToList();

            db.Cars
              .Where(c => c.Price > price)
              .Select(c => new ResultModel
              {
                  FullName = c.Model.Make.Name
              })
              .ToList();

            //var query = EF.CompileQuery<CarDbContext, IEnumerable<ResultModel>>(
            //    db => db.Cars
            //   .Where(c => c.Price > price)
            //   .Select(c => new ResultModel
            //   {
            //       FullName = c.Model.Make.Name
            //   }));

            //var result = query(db);

            CarQueries.ToResult(db, price);

            using var data = new CarDbContext();

            var car = db.Cars.FirstOrDefault();
            data.Attach(car);
            car.Price += 100;

            data.Entry(car).State = EntityState.Detached;
            data.SaveChanges();

            var newCar = new Car { Id = 2 };
            data.Attach(newCar); //with this block we don't make query to searche the object in the database but with Attach and Savachanges we can set the value of the object with this primary key
            newCar.Price = 15000;
            data.SaveChanges();

            //var entry = data.Entry(newCar);
            //entry.State = EntityState.Added;
            //data.SaveChanges();// EXCEPTION  we cannot set explicit value of an existing object 

            //db.SaveChanges();


        }
        private static void LazyLoading(CarDbContext db)
        {
            //using Microsoft.EntityFrameworkCore.Proxies;
            // all refence navigation properties in Models should be virtual to use LazyLoading - WTF
            //in DbContaxt OnConfiguring we have to enable LazyLoading =>  .UseLazyLoadingProxies()

            var car = db.Cars
               .Include(c => c.Model)
               .FirstOrDefault(c => c.Id == 1);

            Console.WriteLine(car.Model.Name);
        }
        private static void EagerLoading(CarDbContext db)
        {
            var car = db.Cars
                .Include(c => c.Model)
                .FirstOrDefault(c => c.Id == 1);

            Console.WriteLine(car.Model.Name);
        }
        private static void ExplicitLoading(CarDbContext db)
        {
            var car = db.Cars.FirstOrDefault(c => c.Id == 1);
            db.Entry(car)
                .Reference(c => c.Model)
                .Load();

            Console.WriteLine(car.Model.Name);
        }
        private static void DeletingAndUpdatingCars(CarDbContext db)
        {
            var cars = db.Cars.Where(c => c.Price > 15000).ToList();
            db.RemoveRange(cars);
            // this make query for every car
          //  db.SaveChanges(); - even without Savechanges 

            var cars2 = db.Cars
                .Where(c => c.Price > 15000);
            /* .Delete();*/ // using Z.EntityFramework.Plus  optimize the delete query

            var cars3 = db.Cars.Where(c => c.Price < 20000);
                //.Update(c => new Car { Price = c.Price * 1.2M });

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
