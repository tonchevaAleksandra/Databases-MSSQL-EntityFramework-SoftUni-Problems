namespace ProductShop.DTO
{
    using Newtonsoft.Json;

    public class ExportUserDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public SoldProductsDto SoldProducts { get; set; }
    }
}
