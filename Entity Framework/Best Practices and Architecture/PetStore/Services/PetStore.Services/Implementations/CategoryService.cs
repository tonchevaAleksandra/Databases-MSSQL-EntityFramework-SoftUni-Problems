using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;

namespace PetStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly PetStoreDbContext data;

        public CategoryService(PetStoreDbContext data)
        {
            this.data = data;
        }

        public void Add(string name, string description)
        {
            if (this.Exists(name))
            {
                throw new ArgumentException("There is already category with given name!");
            }

            var category = new Category()
            {
                Name = name,
                Description = description
            };

            this.data.Categories.Add(category);
            this.data.SaveChanges();
        }

        public bool Exists(int categoryId)
            => this.data.Categories.Any(x => x.Id == categoryId);

        public bool Exists(string name)
            => this.data.Categories.Any(x => x.Name.ToLower() == name.ToLower().Trim());
    }
}
