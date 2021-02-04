using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Toy;

namespace PetStore.Services.Implementations
{
    public class ToyService : IToyService
    {
        private readonly PetStoreDbContext data;
        private readonly UserService userService;

        public ToyService(PetStoreDbContext data, UserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public void BuyToyFromDistributor(string name, string description, decimal price, decimal profit, int brandId, int categoryId)
        {
            ValidateName(name);
            ValidateProfit(profit);
            var toy = new Toy()
            {
                Name = name,
                Description = description,
                DistributorPrice = price,
                Price = price + (price * profit),
                BrandId = brandId,
                CategoryId = categoryId
            };

            this.data.Toys.Add(toy);
            this.data.SaveChanges();
        }

        public void BuyToyFromDistributor(AddingToyServiceModel model)
        {
            ValidateName(model.Name);
            ValidateProfit(model.Profit);

            var toy = new Toy()
            {
                Name = model.Name,
                Description = model.Description,
                DistributorPrice = model.Price,
                Price = model.Price + (model.Price * model.Profit),
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            this.data.Toys.Add(toy);
            this.data.SaveChanges();
        }

        public void SellToyToUser(int toyId, int userId)
        {
            if (!this.Exists(toyId))
            {
                throw new ArgumentException("There is no such toy with the given id in the store!");
            }

            if (!this.userService.Exists(userId))
            {
                throw new ArgumentException("There is no such user witk given id in the database!");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            var toyOrder = new ToyOrder()
            {
                ToyId = toyId,
                Order = order
            };

            this.data.Orders.Add(order);
            this.data.ToyOrders.Add(toyOrder);
            this.data.SaveChanges();
        }

        public bool Exists(int toyId)
            => this.data.Toys.Any(t => t.Id == toyId);

        private void ValidateName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }
        }

        private void ValidateProfit(decimal profit)
        {
            if (profit < 0 || profit > 5)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500 % !");
            }
        }
    }
}
