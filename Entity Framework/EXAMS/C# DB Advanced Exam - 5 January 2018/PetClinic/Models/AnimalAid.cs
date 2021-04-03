using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.Models
{
   public class AnimalAid
    {
        public AnimalAid()
        {
            this.AnimalAidProcedures = new HashSet<ProcedureAnimalAid>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<ProcedureAnimalAid> AnimalAidProcedures  { get; set; }
    }
}
