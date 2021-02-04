using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services;
using PetStore.Web.Models.Pets;

namespace PetStore.Web.Controllers
{
    public class PetsController:Controller
    {
        private readonly IPetService petService;

        public PetsController(IPetService petService)
        {
            this.petService = petService;
        }

        public void All()
        {

        }
    }
}
