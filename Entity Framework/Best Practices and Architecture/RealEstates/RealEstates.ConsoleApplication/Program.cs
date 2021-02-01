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

            //SearchPropertiesInPriceRange(db);
        }

        private static void SearchPropertiesInPriceRange(RealEstateDbContext db)
        {
            IPropertiesService propertiesService = new PropertiesService(db);

            Console.Write("Min price: ");
            int minPrice = int.Parse(Console.ReadLine());
            Console.Write("Max price: ");
            int maxPrice = int.Parse(Console.ReadLine());
            var properties = propertiesService.SearchByPrice(minPrice, maxPrice);

            foreach (var property in properties)
            {
                Console.WriteLine(
                    $"{property.District}, fl. {property.Floor}, {property.Size} m², {property.Year}, {property.Price:f2}€, {property.PropertyType}, {property.BuildingType}");
            }
        }

        private static void PrintTopDistrictsByAveragePrice(RealEstateDbContext db)
        {
            IDistrictService districtService = new DistrictService(db);
            var districts = districtService.GetTopDistrictsByAveragePPrice();

            foreach (var district in districts)
            {
                var propertyCount = district.PropertiesCount == 1 ? "property" : "properties";
                Console.WriteLine(
                    $"{district.Name} => Price: {district.AveragePrice:f2} € ({district.MinPrice} € <-> {district.MaxPrice} €) => {district.PropertiesCount} {propertyCount}");
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
