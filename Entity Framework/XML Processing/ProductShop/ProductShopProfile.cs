using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

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
                .ForMember(x=>x.BuyerName, y=>y.MapFrom(s=>s.Buyer.FirstName + " " + s.Buyer.LastName));

            this.CreateMap<Product, ExportSoldProductsDTO>();

            this.CreateMap<User, ExportUserSoldProductsDTO>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));
        }
    }
}
