using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO.ProductModels;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string ResultsDirectoryPath = "../../../Datasets/Results";
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);

            //Problem 01
            //string inputJson = File.ReadAllText("../../../Datasets/users.json");

            //var result = ImportUsers(db, inputJson);
            //Console.WriteLine(result);

            //Problem 02

            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //var result = ImportProducts(db, inputJson);
            //Console.WriteLine(result);

            //Problem 03
            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //string result = ImportCategories(db, inputJson);
            //Console.WriteLine(result);

            //Problem 04
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //string result = ImportCategoryProducts(db, inputJson);
            //Console.WriteLine(result);

            //Problem 05
           string json=GetProductsInRange(db);
            if (!Directory.Exists(ResultsDirectoryPath))
            {
                Directory.CreateDirectory(ResultsDirectoryPath);
            }
            File.WriteAllText(ResultsDirectoryPath + "/products-in-range.json", json);

        }

        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            //var products = context.Products
            //   .Where(p => p.Price >= 500M && p.Price <= 1000M)
            //   .OrderBy(p => p.Price)
            //   .Select(p => new ListProductsInRangeDTO
            //   {
            //       Name = p.Name,
            //       Price = p.Price.ToString("f2"),
            //       SellerFullName = p.Seller.FirstName + " " + p.Seller.LastName
            //   })
            //   .ToList();

            var products = context.Products
                .Where(p => p.Price >= 500M && p.Price <= 1000M)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price.ToString("f2"),
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            return result;
        }

        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";

        }

        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name != null)
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Problem 01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            int count = users.Count;
            context.SaveChanges();

            return $"Successfully imported {count}";
        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
    }
}