using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.ViewModels
{
   public class BookDTO
    {
        public string Title { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }
         public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }
        public string BookCategories { get; set; }
    }
}
