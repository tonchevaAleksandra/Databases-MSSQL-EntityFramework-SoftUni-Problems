using System;
using System.IO;
using System.Text.Json;

using System.Text.Json.Serialization;
namespace JSON_Demo_Practice
{
    public class StartUp
    {
        static void Main(string[] args)
        {

            WeatherForecast weatherForecast = new WeatherForecast();
            string weatherInfo = JsonSerializer.Serialize(weatherForecast);
            Console.WriteLine(JsonSerializer.Serialize(weatherForecast, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true
            }));

            File.WriteAllText("weather.json", weatherInfo);

            string jsonString = File.ReadAllText("weather.json");
            var weather = JsonSerializer.Deserialize<WeatherForecast>(jsonString);

        }
    }

    class WeatherForecast
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int TemperatureC { get; set; } = 30;
        public string Summary { get; set; } = null;
    }
}
