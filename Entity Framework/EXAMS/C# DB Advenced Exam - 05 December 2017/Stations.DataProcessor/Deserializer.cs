using Stations.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Stations.DataProcessor.Dto.Import;
using Stations.Models;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";

        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportStationDto[] stationDtos = JsonConvert.DeserializeObject<ImportStationDto[]>(jsonString);

            List<Station> stationsToAdd = new List<Station>();

            foreach (var stationDto in stationDtos)
            {
                if (!IsValid(stationDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (stationsToAdd.Any(x=>x.Name==stationDto.Name))
                {
                    continue;
                }
                Station station = new Station()
                {
                    Name = stationDto.Name,
                    Town = stationDto.Town ?? stationDto.Name
                };

                stationsToAdd.Add(station);

                sb.AppendLine(string.Format(SuccessMessage, station.Name));
            }

            context.Stations.AddRange(stationsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            return null;
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            return null;
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            return null;
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            return null;
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            return null;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}