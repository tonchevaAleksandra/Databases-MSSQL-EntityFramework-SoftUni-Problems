namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

    }
}
