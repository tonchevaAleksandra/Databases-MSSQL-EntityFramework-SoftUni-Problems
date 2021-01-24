using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Core.ViewModels.Orders
{
    public class CreateOrderEmployeeViewModel
    {
     
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
