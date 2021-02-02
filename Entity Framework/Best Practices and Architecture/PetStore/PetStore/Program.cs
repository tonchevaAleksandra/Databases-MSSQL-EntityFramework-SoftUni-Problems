using System;
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
        }
    }
}
