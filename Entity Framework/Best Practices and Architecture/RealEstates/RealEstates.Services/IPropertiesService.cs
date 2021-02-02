using System;
using System.Collections.Generic;
using System.Text;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Create(int size, int? floor, int? maxFloor, string district, string propertyType, string buildingType,
            int? year, int price);

        void UpdateTags(int propertyId);

        IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize);

        IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice);

        IEnumerable<PropertyViewModel> SearchByDistrictMaxProperties(string district);
    }
}
