using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static PetStore.Data.Models.DataValidation;

namespace PetStore.Data.Models
{
  public  class Toy
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DistributorPrice { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ToyOrder> Orders { get; set; } = new HashSet<ToyOrder>();
    }
}
