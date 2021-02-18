using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context
                .Prisoners
                .ToList()
                .Where(p => ids.Any(i => i == p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .ToList()
                        .Select(op => new
                        {
                            OfficerName = op.Officer.FullName,
                            Department = op.Officer.Department.Name
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToList(),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisoners = context
                .Prisoners
                .ToArray()
                .Where(p => prisonersNames.Contains(p.FullName))
                .Select(p => new ExportPrisonerDto()
                {
                    Id = p.Id,
                    Name=p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                        .ToArray()
                        .Select(m => new ExportMailDto()
                        {
                            Description = ReverseString(m.Description)
                        })
                        .ToArray()

                })
                .OrderBy(p=>p.Name)
                .ThenBy(p=>p.Id)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportPrisonerDto[]), new XmlRootAttribute("Prisoners"));

            using (StringWriter writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer,prisoners,namespaces);
            }

            return sb.ToString().Trim();

        }

        private static string ReverseString(string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}