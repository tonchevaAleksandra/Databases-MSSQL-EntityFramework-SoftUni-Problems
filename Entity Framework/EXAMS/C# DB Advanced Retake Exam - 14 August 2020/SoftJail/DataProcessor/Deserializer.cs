using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor
{
    using Data;
    using Data.Models;
    using ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyAddedDepartment = "Imported {0} with {1} cells";
        private const string SuccessfullyAddedPrisoner = "Imported {0} {1} years old";
        private const string SuccessfullyAddedOfficer = "Imported {0} ({1} prisoners)";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Department> departments = new List<Department>();

            ImportDepartmentDto[] departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);

            foreach (var departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDto.Name
                };
                List<Cell> cells = new List<Cell>();
                foreach (var cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        cells = new List<Cell>();
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        Department = department,
                        HasWindow = cellDto.HasWindow
                    };
                    cells.Add(cell);

                }

                if (!cells.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                department.Cells = cells;
                departments.Add(department);
                sb.AppendLine(string.Format(SuccessfullyAddedDepartment, department.Name, department.Cells.Count));

            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();

            ImportPrisonerDto[] prisonersDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            foreach (var prisonerDto in prisonersDtos)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime incarcerationDate;
                bool isValidIncDate = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!isValidIncDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? releaseDate;
                if (!String.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    DateTime prisReleaseDate;
                    bool isValidRelDate = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out prisReleaseDate);

                    if (!isValidRelDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    releaseDate = prisReleaseDate;
                }
                else
                {
                    releaseDate = null;
                }

                Prisoner prisoner = new Prisoner()
                {
                    Age = prisonerDto.Age,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId,
                    FullName = prisonerDto.FullName,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Nickname = prisonerDto.Nickname
                };

                List<Mail> mailsToAdd = new List<Mail>();

                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        mailsToAdd = new List<Mail>();
                        break;
                    }

                    Mail mail = new Mail()
                    {
                        Address = mailDto.Address,
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Prisoner = prisoner
                    };
                    mailsToAdd.Add(mail);
                }

                if (!mailsToAdd.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                prisoner.Mails = mailsToAdd;

                prisoners.Add(prisoner);

                sb.AppendLine(String.Format(SuccessfullyAddedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportOfficerDto[]), new XmlRootAttribute("Officers"));

            using (StringReader reader= new StringReader(xmlString))
            {
                ImportOfficerDto[] officerDtos = (ImportOfficerDto[]) xmlSerializer.Deserialize(reader);

                //List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();
                List<Officer> officers = new List<Officer>();

                foreach (var officerDto in officerDtos)
                {
                    if (!IsValid(officerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //if (officers.Any(x => x.FullName == officerDto.FullName))
                    //{
                    //    sb.AppendLine(ErrorMessage);
                    //    continue;
                    //}

                    Position position;
                    bool isValidPosition = Enum.TryParse(officerDto.Position, out position);
                    if(!isValidPosition)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Weapon weapon;
                    bool isValidWeapon = Enum.TryParse(officerDto.Weapon, out weapon);
                    if (!isValidWeapon)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //Department department = context.Departments.FirstOrDefault(x => x.Id == officerDto.DepartmentId);
                    //if (department == null)
                    //{
                    //    sb.AppendLine(ErrorMessage);
                    //    continue;
                    //}

                    Officer officer = new Officer()
                    {
                        DepartmentId = officerDto.DepartmentId,
                        FullName = officerDto.FullName,
                        Position = position,
                        Weapon = weapon,
                        Salary = officerDto.Salary
                    };

                    foreach (var prisonerDto in officerDto.Prisoners)
                    {
                       officer.OfficerPrisoners.Add(new OfficerPrisoner()
                       {
                           Officer = officer,
                           PrisonerId = prisonerDto.Id
                       });
                    }

                    officers.Add(officer);
                    sb.AppendLine(string.Format(SuccessfullyAddedOfficer, officer.FullName,
                        officer.OfficerPrisoners.Count));
                }

                context.Officers.AddRange(officers);
                context.SaveChanges();

                return sb.ToString().Trim();
            }

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