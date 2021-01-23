using AutoMapper;
using System.Linq;

using BookShop.Models;
using BookShop.Data.ViewModels;

namespace BookShop.Data.Profiles
{
   public class AuthorToViewModelProfile:Profile
    {
        public AuthorToViewModelProfile()
        {
            this.CreateMap<Author, AuthorDTO>()
                 .ForMember(x => x.Books, opt => opt.MapFrom(a => string.Join(", ", a.Books.Select(b => b.Title))))
                 .ReverseMap();
        }
    }
}
