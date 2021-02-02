using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static PetStore.Data.Models.DataValidation;

namespace PetStore.Data.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();
        public virtual ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}
