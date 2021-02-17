
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace SoftJail.DataProcessor
{
    using Data;
    using Data.Models;
    using ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyAddedDepartment = "Imported {0} with {1} cells";

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

                //if (!departmentDto.Cells.Any())
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

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
            throw new NotImplementedException();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
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