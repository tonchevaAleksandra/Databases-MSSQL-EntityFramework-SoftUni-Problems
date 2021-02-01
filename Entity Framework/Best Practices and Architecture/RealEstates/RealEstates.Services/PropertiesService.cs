using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public class PropertiesService : IPropertiesService
    {
        private RealEstateDbContext db;

        public PropertiesService(RealEstateDbContext db)
        {
            this.db = db;
        }

        public void Create(int size, int? floor, int? maxFloor, string district, string propertyType, string buildingType, int? year,
            int price)
        {
            var property = new RealEstateProperty
            {
                Size = size,
                Price = price,
                Year = year < 1800 ? null : year,
                Floor = floor <= 0 ? null : floor,
                TotalNumberOfFloors = maxFloor <= 0 ? null : maxFloor,


            };


            //District 
            var districtEntity = db.Districts
                .FirstOrDefault(x => x.Name.Trim() == district.Trim()) ??
                                     new District()
                                     {
                                         Name = district
                                     };

            property.District = districtEntity;

            //Property Type
            var propertyTypeEntity = db.PropertyTypes
                .FirstOrDefault(x => x.Name.Trim() == propertyType.Trim()) ??
                                    new PropertyType()
                                    {
                                        Name = propertyType
                                    };
            property.PropertyType = propertyTypeEntity;

            //Building Type
            var buildingTypeEntity = db.BuildingTypes
                .FirstOrDefault(x => x.Name.Trim() == buildingType.Trim()) ??
                                     new BuildingType()
                                     {
                                         Name = buildingType
                                     };

            property.BuildingType = buildingTypeEntity;

            //TODO: Tags

            this.db.RealEstateProperties.Add(property);
            this.db.SaveChanges();

            this.UpdateTags(property.Id);
        }

        public void UpdateTags(int propertyId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return this.db.RealEstateProperties.Where(x => x.Year >= minYear && x.Year <= maxYear && x.Size >= minSize && x.Size <= maxSize)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Size)
                .ToList();
        }

        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToPropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                Price = x.Price,
                Floor = (x.Floor ?? 0) + "/" + (x.TotalNumberOfFloors ?? 0),
                BuildingType = x.BuildingType.Name,
                District = x.District.Name,
                PropertyType = x.PropertyType.Name,
                Size = x.Size,
                Year = x.Year
            };
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.RealEstateProperties.Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }
    }
}
