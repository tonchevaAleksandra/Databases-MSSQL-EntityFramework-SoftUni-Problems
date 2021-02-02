using System;
using System.Collections.Generic;

namespace PetStore.Data.Models
{
   public class Order
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public OrderStatus Status { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
        public virtual ICollection<FoodOrder> Foods { get; set; } = new HashSet<FoodOrder>();
        public virtual ICollection<ToyOrder> Toys { get; set; } = new HashSet<ToyOrder>();


    }
}
