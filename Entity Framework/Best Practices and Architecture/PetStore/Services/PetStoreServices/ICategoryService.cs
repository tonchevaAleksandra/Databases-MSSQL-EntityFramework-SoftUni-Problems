namespace PetStore.Services
{
   public interface ICategoryService
   {
       void Add(string name, string description);
       bool Exists(int categoryId);

       bool Exists(string name);
   }
}
