using Stations.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";

        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            return null;
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