using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;

namespace PetStore.Services.Implementations
{
   public class BreedService:IBreedService
   {
       private readonly PetStoreDbContext data;

       public BreedService(PetStoreDbContext data)
       {
           this.data = data;
       }

       public void Add(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Breed name cannot be null or whitespace!");
            }

            if (this.Exists(name))
            {
                throw new ArgumentException("There is already breed with that name!");
            }

            var breed = new Breed()
            {
                Name = name
            };

            this.data.Breeds.Add(breed);
            this.data.SaveChanges();

        }

       public bool Exists(string name)
           => this.data.Breeds.Any(x => x.Name.ToLower() == name.ToLower().Trim());

       public bool Exists(int breedId)
           => this.data.Breeds.Any(x => x.Id == breedId);
   }
}
