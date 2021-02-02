using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Data.Models
{
   public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();
        public virtual ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}
