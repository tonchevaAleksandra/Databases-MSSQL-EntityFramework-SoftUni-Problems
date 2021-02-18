using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;

namespace ProductShop
{
    public class StartUp
    {
        private const string DatasetDirPath = @"../../../Datasets/";
        private const string ResultsDirPath = DatasetDirPath + "Results/";
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);
            InitializeMapper();

            //TODO  Problem 01 
            //var inputXml = File.ReadAllText(DatasetDirPath+ "users.xml");
            //var result = ImportUsers(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 02
            //var inputXml = File.ReadAllText(DatasetDirPath + "products.xml");
            //var result = ImportProducts(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 03
            //var inputXml = File.ReadAllText(DatasetDirPath + "categories.xml");
            //var result = ImportCategories(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 04
            //var inputXml = File.ReadAllText(DatasetDirPath + "categories-products.xml");
            //var result = ImportCategoryProducts(db, inputXml);
            //Console.WriteLine(result);

            //TODO Problem 05
            //var result = GetProductsInRange(db);
            //File.WriteAllText(ResultsDirPath + "products-in-range.xml", result);

            //TODO Problem 06
            //var result = GetSoldProducts(db);
            //File.WriteAllText(ResultsDirPath + "users-sold-products.xml", result);

            //TODO Problem 07
            //var result = GetCategoriesByProductsCount(db);
            //File.WriteAllText(ResultsDirPath + "categories-by-products.xml", result);

            //TODO Problem 08
            var result = GetUsersWithProducts(db);
            File.WriteAllText(ResultsDirPath + "users-and-products.xml", result);

        }

        //TODO Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var users = new UserRootDTO()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any(p=>p.Buyer!=null)),
                Users = context.Users
                    .ToArray()
                    .Where(u => u.ProductsSold.Any(p=>p.Buyer!=null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Take(10)
                    .Select(u => new UserExportDTO()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new SoldProductsDTO()
                        {
                            Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                            Products = u.ProductsSold
                                .ToArray()
                                .Where(ps => ps.Buyer != null)
                                .Select(ps => new ExportProductSoldDTO()
                                {
                                    Name = ps.Name,
                                    Price = ps.Price
                                })
                                .OrderByDescending(p => p.Price)
                                .ToArray()
                        }
                    })
                
                    .ToArray()
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserRootDTO), new XmlRootAttribute("Users"));

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var categories = context
                .Categories
                .Select(x=> new ExportCategoryByProductsDTO()
                    {
                    Name = x.Name,
                    AveragePrice = x.CategoryProducts.Average(s=>s.Product.Price),
                    ProductsCount = x.CategoryProducts.Count,
                    TotalRevenue = x.CategoryProducts.Sum(s=>s.Product.Price)
                    })
                .OrderByDescending(c => c.ProductsCount)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportCategoryByProductsDTO[]), new XmlRootAttribute("Categories"));

            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().Trim();


        }

        //TODO Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(x=>x.Buyer!=null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(x=> new ExportUserSoldProductsDTO()
                    {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Where(p=>p.Buyer!=null)
                        .Select(p=> new ExportSoldProductsDTO()
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToArray()

                    })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportUserSoldProductsDTO[]), new XmlRootAttribute("Users"));

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p=> new ExportProductsInRangeDTO()
                {
                    Name = p.Name,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName,
                    Price = p.Price
                    
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportProductsInRangeDTO[]), new XmlRootAttribute("Products"));
          

            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().Trim();
        }

        //TODO Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDTO[]),
                new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDtos =
                (ImportCategoryProductDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            //var categoryProducts = Mapper.Map<CategoryProduct[]>(categoryProductsDtos.Where(cp =>
            //    context.Categories.Any(c => c.Id == cp.CategoryId) && context.Products.Any(p => p.Id == cp.ProductId)));
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDtos)
            {
                if (context.Categories.Any(c => c.Id == categoryProductDto.CategoryId) &&
                    context.Products.Any(p => p.Id == categoryProductDto.ProductId))
                {
                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryProductDto.CategoryId,
                        ProductId = categoryProductDto.ProductId
                    };
                    categoryProducts.Add(categoryProduct);
                }
              
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";

        }

        //TODO Problem 03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportCategoryDTO[]), new XmlRootAttribute("Categories"));

            var categoriesDtos = (ImportCategoryDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            //var categories = Mapper.Map<Category[]>(categoriesDtos.Where(c => c.Name != null));
            List<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDtos)
            {
                Category category = new Category()
                {
                    Name = categoryDto.Name
                };
                if (!categories.Any(c => c.Name == category.Name))
                {
                    categories.Add(category);
                }
                    
            }
            context.Categories.AddRange(categories.Distinct());
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";

        }

        //TODO Problem 02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));

            var productsDtodDtos = (ImportProductDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Product> products = new List<Product>();

            foreach (var productDto in productsDtodDtos)
            {
                Product product = new Product()
                {
                    BuyerId = productDto.BuyerId,
                    Name = productDto.Name,
                    SellerId = productDto.SellerId,
                    Price = productDto.Price
                };
                products.Add(product);
            }

            //var products = Mapper.Map<Product[]>(productsDtodDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";

        }

        //TODO  Problem 01 
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUserDTO[]), new XmlRootAttribute("Users"));

            var usersDtos = (ImportUserDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDtos);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";

        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database is successfully deleted");
            db.Database.EnsureCreated();
            Console.WriteLine("Database is successfully created");
        }
    }
}