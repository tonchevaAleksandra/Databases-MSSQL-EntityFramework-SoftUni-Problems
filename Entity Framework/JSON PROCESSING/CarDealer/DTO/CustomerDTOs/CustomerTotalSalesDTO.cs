using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO.CustomerDTOs
{
  public  class CustomerTotalSalesDTO
    {
        [JsonProperty("fullName")]
        public string Name { get; set; }

        [JsonProperty("boughtCars")]
        public int CarsBought { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }

      
    }
}
