using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Food;

namespace PetStore.Services.Implementations
{
    public class FoodService : IFoodService
    {
        private readonly PetStoreDbContext data;
        private readonly IUserService userService;

        public FoodService(PetStoreDbContext data, IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public void BuyFromDistributor(string name, double weigh, decimal distributorPrice, decimal profit, DateTime expirationDate, int brandId,
             int categoryId)
        {
            ValidateName(name);

            ValidateProfit(profit);

            var food = new Food()
            {
                Name = name,
                Weight = weigh,
                DistributorPrice = distributorPrice,
                Price = distributorPrice + (profit * distributorPrice),
                ExpirationDate = expirationDate,
                BrandId = brandId,
                CategoryId = categoryId
            };

            this.data.Foods.Add(food);
            this.data.SaveChanges();
        }

        public void BuyFromDistributor(AddingFoodServiceModel model)
        {
            ValidateName(model.Name);
            ValidateProfit(model.Profit);

            var food = new Food()
            {
                Name = model.Name,
                Weight = model.Weight,
                DistributorPrice = model.Price,
                Price = model.Price + (model.Profit * model.Price),
                ExpirationDate = model.ExpirationDate,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            this.data.Foods.Add(food);
            this.data.SaveChanges();
        }

        public void SellFoodToUser(int foodId, int userId)
        {
            if (!this.Exists(foodId))
            {
                throw new ArgumentException("There is no such food with the given id in the store!");
            }

            if (!this.userService.Exists(userId))
            {
                throw new ArgumentException("There is no such user with the given id in the database!");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            var foodOrder = new FoodOrder()
            {
                FoodId = foodId,
                Order = order
            };
            this.data.Orders.Add(order);
            this.data.FoodOrders.Add(foodOrder);
            this.data.SaveChanges();
        }

        public bool Exists(int foodId) =>
            this.data.Foods.Any(f => f.Id == foodId);

        private static void ValidateProfit(decimal profit)
        {
            if (profit < 0 || profit > 5.00M)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500%!");
            }
        }

        private static void ValidateName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }
        }
    }
}
