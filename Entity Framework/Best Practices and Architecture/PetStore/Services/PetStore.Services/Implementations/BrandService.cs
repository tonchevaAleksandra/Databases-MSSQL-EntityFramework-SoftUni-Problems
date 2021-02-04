using System;
using System.Linq;
using System.Collections.Generic;

using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Toy;
using PetStore.Services.Models.Brand;

namespace PetStore.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly PetStoreDbContext data;

        public BrandService(PetStoreDbContext data)
            => this.data = data;

        public int Create(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Brand name cannot be null!");
            }
            if (name.Length > DataValidation.NameMaxLength)
            {
                throw new InvalidOperationException($"Brand name cannot be more than {DataValidation.NameMaxLength} characters!");
            }

            if (this.data.Brands.Any(b=>b.Name.ToLower()==name.ToLower()))
            {
                throw new InvalidOperationException($"Brand name already exists!");
            }
            var brand = new Brand()
            {
                Name = name
            };
            this.data.Brands.Add(brand);

            this.data.SaveChanges();

            return brand.Id;
        }

        public IEnumerable<BrandListingServiceModel> SearchByName(string name)
       => this.data.Brands
                .Where(b => b.Name.ToLower().Trim().Contains(name.ToLower().Trim()))
                .Select(b => new BrandListingServiceModel()
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

        public BrandWithToysServiceModel FindByIdWithToys(int id)
            => this.data.Brands
                .Where(b => b.Id == id)
                .Select(b => new BrandWithToysServiceModel()
                {
                    Name = b.Name,
                    Toys = b.Toys.Select(t => new ToyListingServiceModel()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Price = t.Price,
                            TotalOrders = t.Orders.Count
                        })
                        .ToList()
                })
                .FirstOrDefault();

      
    }
}
