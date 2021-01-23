using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.ViewModels
{
  public  class AuthorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BooksCount { get; set; }
        // Book1, ... ,  , ,
        public string Books { get; set; }
    }
}
