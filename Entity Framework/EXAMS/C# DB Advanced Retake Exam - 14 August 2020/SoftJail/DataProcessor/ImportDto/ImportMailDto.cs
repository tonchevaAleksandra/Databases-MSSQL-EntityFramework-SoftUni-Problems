namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportMailDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^((\w*\d*\s*)+(\s{1}str\.))$")]
        public string Address { get; set; }
    }
}
