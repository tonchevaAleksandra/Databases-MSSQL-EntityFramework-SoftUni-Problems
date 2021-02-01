using System;
using System.Collections.Generic;
using System.Text;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
   public interface IDistrictService
   {
       IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePPrice(int count=10);

       IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count=10);
   }
}
