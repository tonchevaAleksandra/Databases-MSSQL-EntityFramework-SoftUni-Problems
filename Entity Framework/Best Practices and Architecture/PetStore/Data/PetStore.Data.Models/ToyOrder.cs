namespace PetStore.Data.Models
{
   public class ToyOrder
    {
        public int ToyId { get; set; }
        public virtual Toy Toy { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
