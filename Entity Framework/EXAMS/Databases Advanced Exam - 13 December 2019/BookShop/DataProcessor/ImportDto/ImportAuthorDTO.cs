namespace BookShop.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportAuthorDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^((\\d){3}-(\\d){3}-(\\d){4})$")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ImportAuthorBookDTO[] Books { get; set; }
    }
}
