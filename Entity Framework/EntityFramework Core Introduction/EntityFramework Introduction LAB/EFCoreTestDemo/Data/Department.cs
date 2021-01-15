using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreTestDemo.Data
{
    public  class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
