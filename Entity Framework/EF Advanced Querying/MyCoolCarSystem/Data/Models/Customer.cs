using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static MyCoolCarSystem.Data.DataValidations.Customer;


namespace MyCoolCarSystem.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxLengthName)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(MaxLengthName)]
        public string LastName { get; set; }

        public int Age { get; set; }
        public Address Address { get; set; }

        public ICollection<CarPurchase> Purchases { get; set; } = new HashSet<CarPurchase>();

    }
}
