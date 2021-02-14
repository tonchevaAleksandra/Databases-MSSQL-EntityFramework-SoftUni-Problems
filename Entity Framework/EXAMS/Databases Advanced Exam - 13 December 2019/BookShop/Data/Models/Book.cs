namespace BookShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
   
    using Enums;

    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public decimal Price { get; set; }

        public int Pages { get; set; }

        public DateTime PublishedOn { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; } = new HashSet<AuthorBook>();


    }
}
