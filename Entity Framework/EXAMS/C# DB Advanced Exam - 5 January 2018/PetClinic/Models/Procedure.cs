using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PetClinic.Models
{
  public  class Procedure
    {
        public Procedure()
        {
            this.ProcedureAnimalAids = new HashSet<ProcedureAnimalAid>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }

        public virtual Animal Animal { get; set; }

        [ForeignKey(nameof(Vet))]
        public int VetId { get; set; }

        public virtual Vet Vet { get; set; }

        public virtual ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }

        [NotMapped] public decimal Cost => this.ProcedureAnimalAids.Sum(x => x.AnimalAid.Price);

        public DateTime DateTime { get; set; }
    }
}
