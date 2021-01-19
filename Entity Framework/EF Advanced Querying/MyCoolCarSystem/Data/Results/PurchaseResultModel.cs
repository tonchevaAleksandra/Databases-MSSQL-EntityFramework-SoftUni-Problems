
using System;

namespace MyCoolCarSystem.Data.Results
{
    public class PurchaseResultModel
    {
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public CarResultModel Car { get; set; }
        public CustomerResultModel Customer { get; set; }
    }
}
