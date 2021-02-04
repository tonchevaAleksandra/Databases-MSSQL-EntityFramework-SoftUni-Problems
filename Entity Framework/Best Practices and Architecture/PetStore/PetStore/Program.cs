using PetStore.Data;
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
        }
    }
}
