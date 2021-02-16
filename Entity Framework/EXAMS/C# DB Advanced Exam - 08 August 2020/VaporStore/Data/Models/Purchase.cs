namespace VaporStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public PurchaseType Type { get; set; }

        [Required]
        public string ProductKey { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        [Required]
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
