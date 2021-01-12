using EFCoreTestDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EFCoreTestDemo
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using var db = new MiniORMContext();
            var employees = db.Employees.ToList();
            foreach (var item in employees)
            {
                Console.WriteLine(item.FirstName + " " + item.LastName + " " + item.IsEmployed);
            }

            db.Departments.Find(1);
            //var employee = new Employee
            //{
            //    FirstName="Aleksandra",
            //    LastName="Toncheva"
            //};

            db.SaveChanges();
            employees = new List<Employee>();
            //--------------------------------
            employees
                .Where(e => e.FirstName.StartsWith("C"));

            Func<Employee, bool> func = e => e.FirstName.StartsWith("C");
            func(new Employee
            {
                FirstName = "Cveta",
                LastName = "SomeName",
                DepartmentId = 1,
                IsEmployed = true
            });
            //--------------------------------
            db.Employees
                .Where(e => e.FirstName.StartsWith("C"));
            Expression<Func<Employee, bool>> expr = e => e.FirstName.StartsWith("C");
            Console.WriteLine(expr.Body);
            //--------------------------------


            var result = db.Departments
                //.ToList() => this will invoke the database but without the filters - that's not the correct way
                .Where(d => d.Name.StartsWith("E"))
                .Where(d => d.Id > 3)
                .Where(d => d.Employees.Count() > 0)
                .Select(d => d.Name)// always use Select statement !!!
                                    //.Skip(()=>1)=> this is betterq but works only in EF 6
                .Take(2)
                .ToList(); //=> this will invoke the database (ToList(),ToArray(),Todictionary(), Max(), Min(), Any(), All(), First(), FirstOrDefault(), Single(), SingleOrDefault(), Count();

            using var db1 = new MiniORMContext();

            var department = new Department
            {
                Name = "IT"
            };

            //department.Employees.Add(new Employee
            //{
            //    FirstName = "Peter",
            //    LastName = "Ivanov"
            //});
            //var emp = db1.Employees.First(e => e.FirstName == "Peter" && e.LastName == "Ivanov");
            //emp.FirstName = "Steve";
            //emp.LastName = "Jobs";
            //db1.Employees.Remove(emp);
            //db1.SaveChanges();

            db1.Employees.Remove(new Employee { Id = 11 });

            //var employee1 = db.Employees.Find(11);
            //employee1.FirstName = "Steve";
            //employee1.LastName = "Jobs";

            var result1 = db.Departments
                .Select(d => new
                {
                    d.Name,
                    Employee = d.Employees.Select(e => e.FirstName)

                })
                .ToList();
            db1.SaveChanges();

            db1.Departments
                .GroupJoin(db.Employees,
                d => d.Id,
                e => e.DepartmentId,
                (d, e) => new
                {
                    Name = d.Name,
                    Employee = e.Select(e => e.FirstName + " " + e.LastName)
                })
                .ToList();


        }


    }
}
