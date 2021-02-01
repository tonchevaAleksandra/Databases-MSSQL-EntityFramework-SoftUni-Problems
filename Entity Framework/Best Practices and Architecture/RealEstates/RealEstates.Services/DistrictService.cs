using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
   public class DistrictService: IDistrictService
    {
       
        public IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePPrice(int count = 10)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count = 10)
        {
            throw new NotImplementedException();
        }
    }
}
