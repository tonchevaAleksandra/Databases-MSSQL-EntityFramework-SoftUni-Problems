using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreTestDemo.Data
{
    public class Project
    {
        public Project()
        {
            EmployeesProjects = new HashSet<EmployeesProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeesProject> EmployeesProjects { get; set; }
    }
}
