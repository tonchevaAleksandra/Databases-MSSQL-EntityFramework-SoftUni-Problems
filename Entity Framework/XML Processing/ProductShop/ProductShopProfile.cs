using System.Linq;

using AutoMapper;

using ProductShop.Models;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDTO, User>();

            this.CreateMap<ImportProductDTO, Product>();

            this.CreateMap<ImportCategoryDTO, Category>();

            this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();

            this.CreateMap<Product, ExportProductsInRangeDTO>()
                .ForMember(x=>x.BuyerName, y=>y.MapFrom(s=>s.Buyer.FirstName + " " + s.Buyer.LastName))
                .ForMember(x=>x.Price, y=>y.MapFrom(z=>z.Price))
                .ForMember(x=>x.Name, y=>y.MapFrom(z=>z.Name));

            this.CreateMap<Product, ExportSoldProductsDTO>();

            this.CreateMap<User, ExportUserSoldProductsDTO>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));

            this.CreateMap<Category, ExportCategoryByProductsDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.ProductsCount, y => y.MapFrom(z => z.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(z => z.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(z => z.CategoryProducts.Sum(cp => cp.Product.Price)));

            this.CreateMap<Product, ExportProductSoldDTO>();


        }
    }
}
