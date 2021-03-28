using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorBookDto
    {
        [JsonProperty("Id")]
        public int? BookId { get; set; }
    }
}
