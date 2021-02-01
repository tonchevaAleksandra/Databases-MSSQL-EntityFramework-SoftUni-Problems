using System;
using System.Collections.Generic;
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
            //var property = new RealEstateProperty
            //{
            //    Size = size,

            //};
        }

      

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            throw new NotImplementedException();
        }
    }
}
