using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stations.Models
{
  public  class Ticket
    {

        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MaxLength(8)]
        public string SeatingPlace { get; set; }

        [ForeignKey(nameof(Trip))]
        public int TripId { get; set; }

        public virtual Trip Trip { get; set; }

        [ForeignKey(nameof(CustomerCard))]
        public int? CustomerCardId { get; set; }

        public virtual CustomerCard CustomerCard { get; set; }

    }
}
