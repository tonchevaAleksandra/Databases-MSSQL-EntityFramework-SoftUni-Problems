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
            if (district == null)
            {
                throw new ArgumentNullException(nameof(district));
                
            }
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
            var property = this.db.RealEstateProperties.FirstOrDefault(x => x.Id == propertyId);

            property.Tags.Clear();

            if (property.Year.HasValue && property.Year < 1990)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag
                    {
                        Tag = this.GetOrCreateTag("OldBuilding")
                    });
            }
            if (property.Year.HasValue && property.Year > 2018 && property.TotalNumberOfFloors.HasValue && property.TotalNumberOfFloors > 5)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag
                    {
                        Tag = this.GetOrCreateTag("HasParking")
                    });
            }

            if (property.Floor.HasValue && property.TotalNumberOfFloors.HasValue
                                        && property.Floor == property.TotalNumberOfFloors)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag
                    {
                        Tag = this.GetOrCreateTag("TopFloor")
                    });
            }

            if (property.Size > 120)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag()
                    {
                        Tag = this.GetOrCreateTag("HugeProperty")
                    });
            }
            if (property.Year.HasValue && property.Year==DateTime.UtcNow.Year)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag()
                    {
                        Tag = this.GetOrCreateTag("NewBuilding")
                    });
            }

            if (property.Floor.HasValue && property.TotalNumberOfFloors.HasValue
            && property.TotalNumberOfFloors>1 && property.Floor==1)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag()
                    {
                        Tag = this.GetOrCreateTag("ParterFloor")
                    });
            }

            if ((double)(property.Price * 1.00 / property.Size) < 800)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag()
                    {
                        Tag = this.GetOrCreateTag("CheepProperty")
                    });
            }
            if ((double)(property.Price * 1.00 / property.Size) > 2000)
            {
                property.Tags
                    .Add(new RealEstatePropertyTag()
                    {
                        Tag = this.GetOrCreateTag("ExpensiveProperty")
                    });
            }

            this.db.SaveChanges();
        }

        private Tag GetOrCreateTag(string tag)
        {
            var tagEntity = this.db.Tags
                .FirstOrDefault(x => x.Name.Trim() == tag.Trim()) ?? new Tag
                {
                    Name = tag
                };

            return tagEntity;
        }

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return this.db.RealEstateProperties.Where(x => x.Year >= minYear && x.Year <= maxYear && x.Size >= minSize && x.Size <= maxSize)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Size)
                .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.RealEstateProperties.Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByDistrictMaxProperties(string district)
        {
            return this.db.RealEstateProperties.Where(x => x.District.Name.Trim() == district.Trim())
                .Select(MapToPropertyViewModel())
                .OrderByDescending(x => x.Price)
                .ToList();
        }

        //public IEnumerable<PropertyViewModel> SearchByDistrict(string district)
        //{
        //    return this.db.RealEstateProperties.Where(x => x.District.Name.Trim()==district.Trim())
        //        .Select(MapToPropertyViewModel())
        //        .OrderByDescending(x => x.Price)
        //        .ToList();
        //}

        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToPropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                Price = x.Price,
                Floor = (x.Floor ?? 0).ToString() /*+ "/" + (x.TotalNumberOfFloors ?? 0).ToString()*/,
                BuildingType = x.BuildingType.Name,
                District = x.District.Name,
                PropertyType = x.PropertyType.Name,
                Size = x.Size,
                Year = x.Year
            };
        }
    }
}
