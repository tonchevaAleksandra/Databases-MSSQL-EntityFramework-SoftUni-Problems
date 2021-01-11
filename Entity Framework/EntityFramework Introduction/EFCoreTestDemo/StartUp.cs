using EFCoreTestDemo.Data;
using System;
using System.Linq;

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
            var employee = new Employee
            {
                FirstName="Aleksandra",
                LastName="Toncheva"
            };
           
            db.SaveChanges();
             employees = db.Employees.ToList();
            foreach (var item in employees)
            {
                Console.WriteLine(item.FirstName + " " + item.LastName + " " + item.IsEmployed);
            }


        }
    }
}
