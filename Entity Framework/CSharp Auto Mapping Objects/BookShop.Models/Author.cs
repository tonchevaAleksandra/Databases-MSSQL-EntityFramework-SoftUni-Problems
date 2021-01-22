namespace BookShop.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Author
    {
        public Author()
        {
            //this.Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}