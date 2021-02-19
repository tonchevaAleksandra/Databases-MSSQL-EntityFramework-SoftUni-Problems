namespace ProductShop.DTO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class UserWithProductsDto
    {
        [JsonProperty("usersCount")]
        public int Count { get; set; }

        [JsonProperty("users")]
        public List<ExportUserDto> Users { get; set; }
    }
}
