using Stations.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class Train
    {
        public Train()
        {
            this.TrainSeats = new HashSet<TrainSeat>();
            this.Trips = new HashSet<Trip>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        public virtual ICollection<TrainSeat> TrainSeats { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}
