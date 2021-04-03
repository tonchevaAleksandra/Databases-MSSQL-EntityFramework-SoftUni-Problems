using Stations.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class CustomerCard
    {
        public CustomerCard()
        {
            this.Type = CardType.Normal;
            this.BoughtTickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public CardType Type { get; set; }

        public virtual ICollection<Ticket> BoughtTickets { get; set; }
    }
}
