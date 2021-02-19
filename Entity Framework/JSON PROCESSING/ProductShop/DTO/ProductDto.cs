namespace ProductShop.DTO
{
    using Newtonsoft.Json;

    public class ProductDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
