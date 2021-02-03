using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static PetStore.Data.Models.DataValidation;

namespace PetStore.Data.Models
{
    public class Food
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        // In kg,
        public double Weight { get; set; }
        public decimal DistributorPrice { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<FoodOrder> Orders { get; set; } = new HashSet<FoodOrder>();
    }
}
