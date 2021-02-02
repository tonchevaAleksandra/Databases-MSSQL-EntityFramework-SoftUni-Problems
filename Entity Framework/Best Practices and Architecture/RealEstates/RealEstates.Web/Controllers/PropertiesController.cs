using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController:Controller
    {
        private readonly IPropertiesService propertiesService;

        public PropertiesController(IPropertiesService propertiesService)
        {
            this.propertiesService = propertiesService;
            this.propertiesService = propertiesService;
        }

        public IActionResult Search()
        {
            return this.View();
        }
        public IActionResult SearchByPriceRange()
        {
            return this.View();
        }

        public IActionResult DoSearchByDistrict()
        {
            
            return this.View();
          
        }

        //public IActionResult SearchByDistrict(string district)
        //{
        //    return this.View();
        //}
        //[Authorize(Roles = "Moderator")]
        public IActionResult DoSearch(int minPrice, int maxPrice)
        {
            //if (!this.User.IsInRole("Admin"))
            //{
            //    return this.BadRequest();
            //}
            var properties = this.propertiesService.SearchByPrice(minPrice, maxPrice);
            return this.View(properties);
        }
    }
}
