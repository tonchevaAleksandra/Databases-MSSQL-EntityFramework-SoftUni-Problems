using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.DTO.CustomerDTOs;
using CarDealer.DTO.SalesDTOs;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {

            this.CreateMap<Customer, CustomerTotalSalesDTO>()
              .ForMember(x => x.Name, y => y.MapFrom(s => s.Name))
              .ForMember(x => x.CarsBought, y => y.MapFrom(s => s.Sales.Count))
              .ForMember(x => x.SpentMoney, y => y.MapFrom(s => s.Sales
                                                      .Select(s => s.Car
                                                                    .PartCars
                                                                    .Select(pc=>pc.Part)
                                                                    .Sum(pc =>pc.Price))
                                                      .Sum()));

            //this.CreateMap<Sale, SaleCarDTO>()
            //    .ForMember(s => s.Make, y => y.MapFrom(s => s.Car.Make))
            //    .ForMember(s => s.Model, y => y.MapFrom(s => s.Car.Model))
            //    .ForMember(s => s.TravelledDistance, y => y.MapFrom(s => s.Car.TravelledDistance));

            this.CreateMap<Car, CarsDTO>();

        }
    }
}
