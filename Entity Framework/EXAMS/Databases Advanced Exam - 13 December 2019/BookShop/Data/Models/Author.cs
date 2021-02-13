namespace BookShop.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        //[MinLength(3)] => cannot be set  on db creation
        public string LastName { get; set; }

        [Required]
        //[EmailAddress]
        public string Email { get; set; }

        [Required]
        //[MaxLength(12)]
        public string Phone { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; } = new HashSet<AuthorBook>();
    }
}
