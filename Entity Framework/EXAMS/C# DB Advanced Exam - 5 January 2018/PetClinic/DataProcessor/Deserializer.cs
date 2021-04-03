using System;
using PetClinic.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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
            StringBuilder sb = new StringBuilder();
            List<string> phoneNumbers = new List<string>();
            List<Vet> vetsToAdd = new List<Vet>();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportVetDto[]), new XmlRootAttribute("Vets"));

            using (StringReader reader= new StringReader(xmlString))
            {
                ImportVetDto[] vetsDtos = (ImportVetDto[]) xmlSerializer.Deserialize(reader);

                foreach (var vetDto in vetsDtos)  
                {
                    if (!IsValid(vetDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (phoneNumbers.Any(x=>x.Equals(vetDto.PhoneNumber)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Vet vet = new Vet()
                    {
                        Age = vetDto.Age,
                        Name = vetDto.Name,
                        PhoneNumber = vetDto.PhoneNumber,
                        Profession = vetDto.Profession
                    };

                    vetsToAdd.Add(vet);
                    sb.AppendLine(string.Format(SuccessMessage, vet.Name));
                }
            }

            context.Vets.AddRange(vetsToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
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