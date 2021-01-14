using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            Console.WriteLine(RemoveTown(context));
        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {

            var town = context.Towns
                .First(t => t.Name == "Seattle");

            var addressesToDel = context
                .Addresses
                .Where(s => s.TownId == town.TownId);
            int addressesCount = addressesToDel.Count();

            var employees = context.Employees
                .Where(e => addressesToDel.Any(a => a.AddressId == e.AddressId));

            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            foreach (var a in addressesToDel)
            {
                context.Addresses.Remove(a);
            }

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addressesCount} addresses in {town.Name} were deleted";
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employeesProjectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            var project = context.Projects
                .Where(p => p.ProjectId == 2)
                .Single();

            foreach (var ep in employeesProjectsToDelete)
            {
                context.EmployeesProjects.Remove(ep);
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(p => sb.AppendLine(p));

            return sb.ToString().Trim();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));

            return sb.ToString().Trim();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                 .Where(e => new[] { "Engineering", "Tool Design", "Marketing", "Information Services" }
                        .Contains(e.Department.Name))
                 .ToList()
                 .ForEach(e => e.Salary *= 1.12M);

            context.SaveChanges();

            context.Employees
                 .Where(e => new[] { "Engineering", "Tool Design", "Marketing", "Information Services" }
                          .Contains(e.Department.Name))
                 .Select(e => new
                 {
                     e.FirstName,
                     e.LastName,
                     e.Salary
                 })
                 .OrderBy(e => e.FirstName)
                 .ThenBy(e => e.LastName)
                 .ToList()
                 .ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})"));

            return sb.ToString().Trim();

        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .OrderBy(p => p.ProjectName)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine(p.ProjectName)
                    .AppendLine(p.Description)
                    .AppendLine(p.StartDate);
            }

            return sb.ToString().Trim();
        }

        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employeеs = d.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()
                })
                .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.Employeеs)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().Trim();

        }

        //Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                    .Select(ep => ep.Project.Name)
                    .OrderBy(pn => pn)
                    .ToList()
                })
                .Single();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} -  {employee.JobTitle}");

            foreach (var projectName in employee.Projects)
            {
                sb.AppendLine(projectName);
            }

            return sb.ToString().Trim();
        }

        //Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .ToList();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().Trim();

        }

        //Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue ?
                        ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                    })
                    .ToList()
                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);// EFC will added even without ythat command
            Employee employee = context.Employees
                .First(e => e.LastName == "Nakov");
            employee.Address = newAddress;

            //context.SaveChanges();

            List<string> employeesAdresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                 .Take(10)
                .Select(e => e.Address.AddressText)
                 .ToList();

            foreach (string address in employeesAdresses)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().Trim();
        }

        //Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} from {item.Department} - ${item.Salary:f2}");
            }
            return sb.ToString().Trim();

        }

        //Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,

                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var item in employees)
            {
                sb.AppendLine(item.FirstName + $" - {item.Salary:f2}");
            }

            return sb.ToString().Trim();

        }

        //Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
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

            return sb.ToString().Trim();
        }
    }
}
