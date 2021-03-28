using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-90]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public AuthorBookDto[] Books { get; set; }
    }
}
