using System;
using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RealEstateDbContext();
            //RestoreDatabase(db);
            db.Database.Migrate();

            //IPropertiesService propertiesService = new PropertiesService(db);
            //propertiesService.Create(120, 16, 20, "Дианабад", "4-СТАЕН", "ЕПК", 2018, 200000);


        }

        private static void RestoreDatabase(RealEstateDbContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database is successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database is successfully created!");
        }
    }
}
