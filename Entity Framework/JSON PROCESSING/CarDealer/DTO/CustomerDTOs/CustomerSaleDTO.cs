using CarDealer.DTO.SalesDTOs;
using Newtonsoft.Json;


namespace CarDealer.DTO.CustomerDTOs
{
   public class CustomerSaleDTO
    {
        [JsonProperty("car")]
        public SaleCarDTO Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        public decimal Discount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public decimal PriceWithDiscount { get; set; }

      
    }
}
