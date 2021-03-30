using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }
    }
}
