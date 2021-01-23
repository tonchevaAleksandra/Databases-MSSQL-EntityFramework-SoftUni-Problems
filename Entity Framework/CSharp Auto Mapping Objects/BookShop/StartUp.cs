namespace BookShop
{
    using BookShop.Data.ViewModels;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookShop.Data.Profiles;

    public class StartUp
    {
        public static void Main()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Book, BookDTO>()
            //    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));
            //}); //AutoMapper version 8.0

            //DbInitializer.ResetDatabase(db);

            //var book = db.Books.First();
            //var bookDto = Mapper.Map<BookDTO>(book);
            //var books = db.Books
            //    .Select(b=>new BookDTO()
            //    {
            //        Title = b.Title,
            //        Price = b.Price,
            //        Author = $"{b.Author.FirstName} {b.Author.LastName}"
            //    })
            //    .ToList();



            //var bookDto = new BookDTO()
            //{
            //    Title = book.Title,
            //    Price = book.Price,
            //    Author = $"{book.Author.FirstName} {book.Author.LastName}"
            //};

            //string result = JsonConvert.SerializeObject(bookDto, Formatting.Indented);
            //Console.WriteLine(result); 


            //var author = db.Authors.Select(x => new AuthorDTO
            //{
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    Books = x.Books.Count()
            //})
            //    .FirstOrDefault();

            //Print(author);

            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            var config = new MapperConfiguration(cfg =>
             {
                 //cfg.CreateMap<Author, AuthorDTO>()
                 //.ForMember(x => x.Books, opt => opt.MapFrom(a => string.Join(", ", a.Books.Select(b => b.Title))))
                 //.ReverseMap();
                 cfg.AddProfile(new AuthorToViewModelProfile());

                 cfg.CreateMap<Book, BookDTO>()
                 //.ForMember(x=>x.Author, opt=>opt.MapFrom(b=>string.Concat(b.Author.FirstName, " ", b.Author.LastName)))
                 .ForMember(x => x.BookCategories, opt => opt.MapFrom(b => string.Join(", ", b.BookCategories.Select(c => c.Category.Name))))
                 .ReverseMap();
                 cfg.CreateMap<Category, CategoryDTO>();
             });
            IMapper mapper = config.CreateMapper();


            var inputModel = new BookDTO { Title = "Windows on the world", Price = 14M, AuthorFirstName = "Frederic", AuthorLastName = "Beigbeder", Description="Some description about the book" };
            var bookInput = mapper.Map<Book>(inputModel);

            
           
            Print(bookInput);//{
                             //            "BookId": 0,
                             //  "Title": "Windows on the world",
                             //  "Description": null,
                             //  "EditionType": 0,
                             //  "Price": 14.0,
                             //  "Copies": 0,
                             //  "ReleaseDate": null,
                             //  "AgeRestriction": 0,
                             //  "AuthorId": 0,
                             //  "Author": {
                             //                "AuthorId": 0,
                             //    "FirstName": "Frederic",
                             //    "LastName": "Beigbeder",
                             //    "Books": []
                             //  },
                             //  "BookCategories": []
                             //}

            var authorInput = new AuthorDTO { FirstName = "Frederic", LastName = "Beigbeder"};
            var authorToMap = mapper.Map<Author>(authorInput);
            //db.Authors.Add(authorToMap);
            //db.SaveChanges();
            Print(authorToMap);

            Console.WriteLine("-----------------");
            var author = db.Authors.FirstOrDefault();
            var authorModel = db.Authors
                .ProjectTo<AuthorDTO>(config);
            Print(authorModel);

            var bookModel = db.Books/*.Where(x => x.BookId == 1)*/
                .ProjectTo<BookDTO>(config)/*.FirstOrDefault()*/;
            Print(bookModel);

            var categoryModel = db.Categories.Where(x => x.CategoryId == 3)
                .ProjectTo<CategoryDTO>(config).FirstOrDefault();
            Print(categoryModel);

        }

        private static void Print(object authors)
        {
            Console.WriteLine(JsonConvert.SerializeObject(authors, Formatting.Indented));
        }


    }
}
