using System;
using System.Collections.Generic;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Pet;

namespace PetStore.Services.Implementations
{
    public class PetService : IPetService
    {
        private readonly PetStoreDbContext data;
        private readonly ICategoryService categoryService;
        private readonly IBreedService breedService;
        private readonly IUserService userService;

        public PetService(PetStoreDbContext data, CategoryService categoryService, BreedService breedService, IUserService userService)
        {
            this.data = data;
            this.categoryService = categoryService;
            this.breedService = breedService;
            this.userService = userService;
        }

        public IEnumerable<PetListingServiceModel> All()
            => this.data.Pets.Select(p => new PetListingServiceModel
            {
                Id = p.Id,
                Category = p.Category.Name,
                Breed = p.Breed.Name,
                Price = p.Price
            })
                .ToList();

        public void BuyPet(Gender gender, DateTime dateOfBirth, decimal price, string description, int breedId, int categoryId)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price of the pet cannot be less than zero!");
            }

            if (!this.breedService.Exists(breedId))
            {
                throw new ArgumentException("There is no such breed with given id in database!");
            }

            if (!this.categoryService.Exists(categoryId))
            {
                throw new ArgumentException("There is no such category with given id in database!");
            }

            var pet = new Pet()
            {
                BreedId = breedId,
                CategoryId = categoryId,
                DateOfBirth = DateTime.Now,
                Description = description,
                Gender = gender,
                Price = price
            };

            this.data.Pets.Add(pet);
            this.data.SaveChanges();
        }

        public void SellPet(int petId, int userId)
        {
            if (!this.Exists(petId))
                throw new ArgumentException("There is no such pet with given id!");


            if (!this.userService.Exists(userId))
                throw new ArgumentException("There is no such user with given id!");

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            this.data.Orders.Add(order);

            this.data.Pets.Find(petId).Order = order;

            this.data.SaveChanges();
        }

        public bool Exists(int petId)
            => this.data.Pets.Any(x => x.Id == petId);
    }
}
