using System.IO.Compression;
using System.Linq;
using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDTO, Supplier>();

            this.CreateMap<ImportPartsDTO, Part>();

            this.CreateMap<ImportCustomerDTO, Customer>();

            this.CreateMap<ImportSaleDTO, Sale>();

            this.CreateMap<Supplier, ExportLocalSuppliersDTO>();

            this.CreateMap<Car, ExportCarWithDistanceDTO>();

            this.CreateMap<Car, ExportCarsBMWDTO>();

            this.CreateMap<PartCar, ExportPartCarsDTO>()
                .ForMember(pc=>pc.Name, p=>p.MapFrom(pc=>pc.Part.Name))
                .ForMember(pc=>pc.Price, c=>c.MapFrom(pc=>pc.Part.Price));

            this.CreateMap<Car, ExportCarWithPartsDTO>()
                .ForMember(x=>x.Parts, y=>y.MapFrom(s=>s.PartCars.OrderByDescending(pc => pc.Part.Price)));

            this.CreateMap<Customer, ExportCustomerTotalSalesDTO>()
                .ForMember(c => c.BoughtCars, y => y.MapFrom(s => s.Sales.Count))
                .ForMember(c => c.SpentMoney,
                    y => y.MapFrom(c => c.Sales.Select(s => s.Car.PartCars.Sum(pc => pc.Part.Price)).Sum()));

            this.CreateMap<Car, ExportCarSaleDTO>();

            this.CreateMap<Sale, ExportSaleDTO>()
                .ForMember(x => x.Car, y => y.MapFrom(s => s.Car))
                .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.Discount, y => y.MapFrom(s => s.Discount))
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Car.PartCars.Select(pc => pc.Part.Price).Sum()))
                .ForMember(x => x.PriceWithDiscount,
                    y => y.MapFrom(s =>
                        s.Car.PartCars.Select(pc => pc.Part.Price).Sum() -
                        s.Discount / 100 * s.Car.PartCars.Select(pc => pc.Part.Price).Sum()));
        }
    }
}
