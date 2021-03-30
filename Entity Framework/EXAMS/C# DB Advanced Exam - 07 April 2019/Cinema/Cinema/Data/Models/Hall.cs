using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Hall
    {
        public Hall()
        {
            this.Projections = new HashSet<Projection>();
            this.Seats = new HashSet<Seat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public virtual  ICollection<Seat> Seats { get; set; }

    }
}
