using PetClinic.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.DataProcessor
{
    public class Deserializer
    {
        private const string SuccessMessage = "Record {0} successfully imported.";
        private const string SuccessMessageAnimal = "Record {0} Passport №: {1} successfully imported.";
        private const string SuccessMessageProcedure = "Record successfully imported.";
        private const string ErrorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            return null;
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            return null;
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            return null;
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            return null;
        }

        public static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}