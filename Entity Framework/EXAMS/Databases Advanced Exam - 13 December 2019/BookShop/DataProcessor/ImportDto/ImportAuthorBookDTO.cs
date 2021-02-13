namespace BookShop.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    public class ImportAuthorBookDTO
    {
        [JsonProperty("Id")]
        public int? BookId { get; set; }
    }
}
