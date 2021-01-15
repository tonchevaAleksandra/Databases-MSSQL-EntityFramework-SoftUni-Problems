using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        public int StoreId { get; set; }

        [MaxLength(80)]
        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
