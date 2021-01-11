using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreTestDemo.Data
{
    public  class Employee
    {
        public Employee()
        {
            EmployeesProjects = new HashSet<EmployeesProject>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsEmployed { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<EmployeesProject> EmployeesProjects { get; set; }
    }
}
