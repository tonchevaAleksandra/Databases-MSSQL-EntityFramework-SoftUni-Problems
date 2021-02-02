using System.Collections.Generic;

namespace PetStore.Data.Models
{

    //Each category is for each animal
   public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
        public virtual ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();
        public virtual ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}
