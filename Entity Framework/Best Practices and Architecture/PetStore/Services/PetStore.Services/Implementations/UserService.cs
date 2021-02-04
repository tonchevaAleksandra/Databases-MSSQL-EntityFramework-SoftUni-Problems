using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;

namespace PetStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly PetStoreDbContext data;

        public UserService(PetStoreDbContext data)
        {
            this.data = data;
        }

        public bool Exists(int userId) =>
            this.data.Users.Any(x => x.Id == userId);

        public int RegisterUser(string name, string email)
        {
            var user = new User()
            {
                Name = name,
                Email = email
            };

            this.data.Users.Add(user);
            this.data.SaveChanges();
            return user.Id;
        }
    }
}
