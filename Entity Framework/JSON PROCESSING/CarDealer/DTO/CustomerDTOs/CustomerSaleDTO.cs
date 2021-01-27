using CarDealer.DTO.SalesDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO.CustomerDTOs
{
   public class CustomerSaleDTO
    {
        [JsonProperty("car")]
        public SaleCarDTO Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        public string Discount { get; set; }

        [JsonProperty("price")]
        public string  Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount { get; set; }
    }
}
