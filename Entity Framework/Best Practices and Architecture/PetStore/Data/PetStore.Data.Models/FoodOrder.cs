using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Models
{
   public class FoodOrder
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
    }
}
