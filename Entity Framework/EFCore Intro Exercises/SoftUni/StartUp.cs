using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e=>e.Salary>50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new Employee
                {
                    FirstName = e.FirstName,
                   
                    Salary = e.Salary
                })
                .ToList();

            foreach (var item in employees)
            {
                sb.AppendLine(item.FirstName + $" - {item.Salary:f2}");
            }

            return sb.ToString();

        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new Employee
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .ToList();

            foreach (var item in employees)
            {
                sb.AppendLine(item.FirstName + " " + item.LastName + " " + item.MiddleName + " " + item.JobTitle + " " + $"{item.Salary:f2}");
            }

            return sb.ToString();
        }
    }
}
