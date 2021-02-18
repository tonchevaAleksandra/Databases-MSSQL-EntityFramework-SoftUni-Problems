using Newtonsoft.Json;

namespace CarDealer.DTO
{
    public class ImportCarDto
    {
        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("travelledDistance")]
        public long TravelledDistance { get; set; }
   
        public int[] partsId { get; set; }
    }
}
