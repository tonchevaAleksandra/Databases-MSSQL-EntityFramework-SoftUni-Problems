﻿using AutoMapper;
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
        }
    }
}