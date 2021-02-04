using PetStore.Services.Models.Toy;

namespace PetStore.Services
{
   public interface IToyService
   {
       void BuyToyFromDistributor(string name, string description , decimal price, decimal profit, int brandId,
           int categoryId);

       void BuyToyFromDistributor(AddingToyServiceModel model);

       void SellToyToUser(int toyId, int userId);

       bool Exists(int toyId);
   }
}
