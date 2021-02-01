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

            //AddCustomPropertiesToDatabase(db);

            //PrintTopDistrictsByAveragePrice(db);
        }

        private static void PrintTopDistrictsByAveragePrice(RealEstateDbContext db)
        {
            IDistrictService districtService = new DistrictService(db);
            var districts = districtService.GetTopDistrictsByAveragePPrice();

            foreach (var district in districts)
            {
                var propertyCount = district.PropertiesCount == 1 ? "property" : "properties";
                Console.WriteLine(
                    $"{district.Name} => Price: {district.AveragePrice:f2} EUR ({district.MinPrice} EUR <-> {district.MaxPrice} EUR) => {district.PropertiesCount} {propertyCount}");
            }
        }

        private static void AddCustomPropertiesToDatabase(RealEstateDbContext db)
        {
            IPropertiesService propertiesService = new PropertiesService(db);
            propertiesService.Create(125, 6, 6, "Дианабад", "4-СТАЕН", "Tyxла", 2021, 2000000);

            propertiesService.UpdateTags(1);
            propertiesService.UpdateTags(2);
            propertiesService.UpdateTags(3);
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
