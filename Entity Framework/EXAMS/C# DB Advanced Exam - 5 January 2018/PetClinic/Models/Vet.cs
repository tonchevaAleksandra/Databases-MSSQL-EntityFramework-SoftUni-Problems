using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.Models
{
    public class Vet
    {
        public Vet()
        {
            this.Procedures = new HashSet<Procedure>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Profession { get; set; }

        public int Age { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}
