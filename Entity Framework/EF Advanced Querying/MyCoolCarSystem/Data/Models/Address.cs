using System.ComponentModel.DataAnnotations;

using static MyCoolCarSystem.Data.DataValidations.Address;

namespace MyCoolCarSystem.Data.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTownLength)]
        public string Town { get; set; }

        [Required]
        [MaxLength(MaxTextLength)]
        public string Text { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
