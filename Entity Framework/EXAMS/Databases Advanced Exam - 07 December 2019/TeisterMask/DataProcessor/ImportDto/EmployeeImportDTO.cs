namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    public class EmployeeImportDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^((\d){3}-(\d){3}-(\d){4})$")]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }
    }
}
