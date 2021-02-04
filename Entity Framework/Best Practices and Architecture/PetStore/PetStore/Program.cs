using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Implementations;

namespace PetStore
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var data = new PetStoreDbContext();
            var brandService = new BrandService(data);

            //brandService.Create("Purrina");

            //var brandWithToys = brandService.FindByIdWithToys(1);

            //var foodService = new FoodService(data);
            //foodService.BuyFromDistributor("Cat food", 0.350, 1.10M, 0.3M, DateTime.Now, 1, 1);

            //var toyService = new ToyService(data);
            //toyService.BuyToyFromDistributor("Ball", null, 3.50M, 0.3M, 1,1);

            var userService = new UserService(data);
            //var foodService = new FoodService(data, userService);
            //int userId = userService.RegisterUser("Pesho", "pesho123@gmail.com");
            //foodService.SellFoodToUser(1, userId);

            //var toyService = new ToyService(data, userService);
            //toyService.SellToyToUser(1,1);

            //var breedService = new BreedService(data);
            //breedService.Add("Persian");

            //var petService = new PetService(data, new CategoryService(data), breedService, userService);
            //petService.BuyPet(Gender.Female, DateTime.Now.AddDays(-30), 0M, "small kitten with no habits", 1, 1);
            //petService.SellPet(1,1);

            //SeedDatabaseWithPetsCtagoriesAndBreeds(data);
          

        }

        private static void SeedDatabaseWithPetsCtagoriesAndBreeds(PetStoreDbContext data)
        {
            for (int i = 0; i < 10; i++)
            {
                var breed = new Breed()
                {
                    Name = "Breed " + i,
                };

                data.Breeds.Add(breed);
            }

            data.SaveChanges();

            for (int i = 0; i < 30; i++)
            {
                var category = new Category()
                {
                    Name = "Category " + i,
                    Description = "Description " + i
                };
                data.Categories.Add(category);
            }

            data.SaveChanges();

            for (int i = 0; i < 100; i++)
            {
                var categoryId = data
                    .Categories
                    .OrderBy(c => Guid.NewGuid())
                    .Select(c => c.Id)
                    .FirstOrDefault();

                var breedId = data.Breeds
                    .OrderBy(b => Guid.NewGuid())
                    .Select(b => b.Id)
                    .FirstOrDefault();

                var pet = new Pet()
                {
                    DateOfBirth = DateTime.Now.AddDays(-60 + i),
                    Price = 50 + i,
                    Gender = (Gender) (i % 2),
                    Description = "Some random petDescription",
                    CategoryId = categoryId,
                    BreedId = breedId
                };

                data.Pets.Add(pet);
            }


            data.SaveChanges();
        }
    }
}
