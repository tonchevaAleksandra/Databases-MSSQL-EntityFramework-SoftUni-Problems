using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PetClinic.Data;
using PetClinic.DataProcessor.Dto.Export;

namespace PetClinic.DataProcessor
{
    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(x => x.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(x => new
                {
                    OwnerName = x.Passport.OwnerName,
                    AnimalName = x.Name,
                    Age = x.Age,
                    SerialNumber = x.Passport.SerialNumber,
                    RegisteredOn = x.Passport.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                })
                .OrderByDescending(x => x.Age)
                .ThenBy(x => x.SerialNumber)
                .ToList();

            string json = JsonConvert.SerializeObject(animals, Formatting.Indented);

            return json;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            var procedures = context.Procedures
                .Select(x => new ExportProcedureDto()
                {
                    Passport = x.Animal.PassportSerialNumber,
                    OwnerNumber = x.Animal.Passport.OwnerPhoneNumber,
                    DateTime = x.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = x.ProcedureAnimalAids.Select(y => new ExportAnimalAidDto()
                        {
                            Name = y.AnimalAid.Name,
                            Price = y.AnimalAid.Price
                        })
                        .ToArray(),
                    TotalPrice = x.ProcedureAnimalAids.Sum(y => y.AnimalAid.Price)
                })
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.Passport)
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportProcedureDto[]), new XmlRootAttribute("Procedures"));

            using (StringWriter writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, procedures, namespaces);
            }

            return sb.ToString().Trim();
        }
    }
}