using System;

namespace PetStore.Data.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public Gender Gender { get; set; }
        public int BreedId { get; set; }
        public virtual Breed Breed { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }


    }
}
