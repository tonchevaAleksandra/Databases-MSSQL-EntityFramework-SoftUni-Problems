namespace PetStore.Services
{
   public interface IUserService
   {
       bool Exists(int userId);
       int RegisterUser(string name, string email);
   }
}
