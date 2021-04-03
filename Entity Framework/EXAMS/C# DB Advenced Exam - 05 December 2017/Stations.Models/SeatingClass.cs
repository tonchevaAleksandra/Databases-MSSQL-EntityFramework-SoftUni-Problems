using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class SeatingClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string Abbreviation { get; set; }
    }
}
