using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stations.Models
{
  public  class TrainSeat
    {
        [Key]
        public int Id { get; set; }

        public int TrainId { get; set; }

        public virtual Train Train { get; set; }

        public int SeatingClassId { get; set; }

        public virtual SeatingClass SeatingClass { get; set; }

        public int Quantity { get; set; }
    }
}
