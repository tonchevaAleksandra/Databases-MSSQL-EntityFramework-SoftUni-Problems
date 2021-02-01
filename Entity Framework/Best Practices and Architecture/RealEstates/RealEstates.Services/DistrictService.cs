using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
   public class DistrictService: IDistrictService
    {
        private RealEstateDbContext db;

        public DistrictService(RealEstateDbContext db)
        {
            this.db = db;
        }
        public IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePPrice(int count = 10)
        {
            return this.db.Districts.OrderByDescending(x => x.Properties.Average(p => p.Price))
                .Select(MapToDistrictViewModel())
                .Take(count)
                .ToList();
        }


        public IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count = 10)
        {
            return this.db.Districts.OrderByDescending(x => x.Properties.Count)
                .Select(MapToDistrictViewModel())
                .Take(count)
                .ToList();
        }
        private static Expression<Func<District, DistrictViewModel>> MapToDistrictViewModel()
        {
            return x=> new DistrictViewModel()
            {
                AveragePrice = x.Properties.Average(p=>p.Price/p.Size),
                MaxPrice = x.Properties.Max(p=>p.Price/p.Size),
                MinPrice = x.Properties.Min(p=>p.Price/p.Size),
                Name=x.Name,
                PropertiesCount = x.Properties.Count()
            };
        }
    }
}
