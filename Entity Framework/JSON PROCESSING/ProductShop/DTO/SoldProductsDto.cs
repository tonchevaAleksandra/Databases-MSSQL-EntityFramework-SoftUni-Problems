namespace ProductShop.DTO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SoldProductsDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public List<ProductDto> Products { get; set; }
    }
}
