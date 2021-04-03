using System;
using PetClinic.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using PetClinic.DataProcessor.Dto.Import;
using PetClinic.Models;

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
            StringBuilder sb = new StringBuilder();
            List<AnimalAid> animalAidsToAdd = new List<AnimalAid>();
            ImportAnimalAidDto[] animalAidDtos = JsonConvert.DeserializeObject<ImportAnimalAidDto[]>(jsonString);

            foreach (var animalAidDto in animalAidDtos)
            {
                if (!IsValid(animalAidDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (animalAidsToAdd.Any(x=>x.Name==animalAidDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                AnimalAid animalAid = new AnimalAid()
                {
                    Name = animalAidDto.Name,
                    Price = animalAidDto.Price
                };

                animalAidsToAdd.Add(animalAid);
                sb.AppendLine(String.Format(SuccessMessage, animalAid.Name));
            }

            context.AnimalAids.AddRange(animalAidsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Passport> passports = new List<Passport>();
            List<Animal> animalsToAdd = new List<Animal>();

            ImportAnimalDto[] animalDtos = JsonConvert.DeserializeObject<ImportAnimalDto[]>(jsonString);

            foreach (var animalDto in animalDtos)
            {
                if (!IsValid(animalDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(animalDto.Passport))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (passports.Any(x=>x.SerialNumber==animalDto.Passport.SerialNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Animal animal = new Animal()
                {
                    Age = animalDto.Age,
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                };

                DateTime date;
                bool isValidDate = DateTime.TryParseExact(animalDto.Passport.RegistrationDate, "MM-dd-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Passport passport = new Passport()
                {
                    OwnerName = animalDto.Passport.OwnerName,
                    Animal = animal,
                    OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                    SerialNumber = animalDto.Passport.SerialNumber,
                    RegistrationDate = date
                };
                animal.Passport = passport;
                passports.Add(passport);
                animalsToAdd.Add(animal);

                sb.AppendLine(String.Format(SuccessMessageAnimal, animalDto.Name, animalDto.Passport.SerialNumber));
            }

            context.Passports.AddRange(passports);
            context.Animals.AddRange(animalsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
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