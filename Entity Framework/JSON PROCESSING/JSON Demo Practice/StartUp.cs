using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

using System.Text.Json.Serialization;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace JSON_Demo_Practice
{
    class WeatherForecast
    {
        public double LongNameOfThisDecimalProperty { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        /* [JsonProperty("date_of_the_forecast", Order =1)]*///second
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /*  [JsonProperty(Order =2)]*///third
        public int[] TemperaturesC { get; set; } = new[] { 25, 30, 32 };

        /* [JsonProperty(Order =0)]*///first
        //[JsonProperty(Required = Required.Default)]
        //[JsonRequired]
        //[JsonPropertyName("some_name")]
        public string Summary { get; set; } = "Hot summer day";

    }
    class Forecasts
    {
        public Tuple<int, string>/*(int IntValue, string StringValue)*/ AdditionalData { get; set; } 

        public List<WeatherForecast> WeatherForecasts { get; set; }
    }
    public class StartUp
    {
        static void Main(string[] args)
        {
            //UseSystemTextJson();
            //UsingNewtonSoft();

            //ConvertComplexObject();

            //ConvertAnonymousType();

            //var contractResolver = new DefaultContractResolver
            //{
            //    NamingStrategy = new SnakeCaseNamingStrategy()
            //};
            //UsingJsonSettings();

            //UsingLinqOnJson();

            string xml= @"<?xml version='1.0' standalone='no'?> 
                                 <root> 
                                    <person id='1'> 
                                        <name>Alan</name> 
                                        <url>www.google.com</url> 
                                    </person> 
                                    <person id='2'> 
                                        <name>Louis</name> 
                                        <url>www.yahoo.com</url> 
                                    </person> 
                                </root>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
            Console.WriteLine(jsonText);
        }

        private static void UsingLinqOnJson()
        {
            var json = File.ReadAllText("weather1.json");
            JObject jObject = JObject.Parse(json);
            //Console.WriteLine(jObject["connectionString"].Children()[0]);
            foreach (var item in jObject["connectionString"].Where(x => x["ConnectionString"].ToString().Contains("Server")))
            {
                Console.WriteLine(item);
            }

            //foreach (var item in jObject.Children())
            //{
            //    Console.WriteLine(item);
            //    foreach (var item2 in item.Children())
            //    {
            //        Console.WriteLine("--"+item2);//values
            //    }
            //}
        }

        private static void UsingJsonSettings()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new KebabCaseNamingStrategy()
                },
                Culture = CultureInfo.InvariantCulture,
                DateFormatString = "yyyy-MM-dd"
            };
            var forecast = new WeatherForecast();
            Console.WriteLine(JsonConvert.SerializeObject(forecast, jsonSettings));
        }

        private static void ConvertAnonymousType()
        {
            var obj = new
            {
                TemperatureC = 0,
                Summary = string.Empty
            };
            var json = File.ReadAllText("weather.json");
            obj = JsonConvert.DeserializeAnonymousType(json, obj);
        }

        private static void ConvertComplexObject()
        {
            var weather = new Forecasts
            {
                AdditionalData = new Tuple<int, string>(123, "SomeData"),
                WeatherForecasts = new List<WeatherForecast> { new WeatherForecast(), new WeatherForecast(), new WeatherForecast() }
            };
            Console.WriteLine(JsonConvert.SerializeObject(weather, Formatting.Indented));
        }

        private static void UsingNewtonSoft()
        {
            var weather = new WeatherForecast();
            Console.WriteLine(JsonConvert.SerializeObject(weather, Formatting.Indented));

            var json = File.ReadAllText("weather.json");
            var weather1 = JsonConvert.DeserializeObject<WeatherForecast>(json);
        }

        private static void UseSystemTextJson()
        {
            WeatherForecast weatherForecast = new WeatherForecast();
            string weatherInfo = System.Text.Json.JsonSerializer.Serialize(weatherForecast);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(weatherForecast, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
                AllowTrailingCommas = true
            }));

            File.WriteAllText("weather.json", weatherInfo);

            string jsonString = File.ReadAllText("weather.json");
            var weather = System.Text.Json.JsonSerializer.Deserialize<WeatherForecast>(jsonString);


        }
    }

}
