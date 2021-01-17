using Microsoft.AspNetCore.Mvc;
using P03_FootballBetting.Data;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P03_FootballBetting.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly FootballBettingContext footballBettingContext;

        public UsersController(FootballBettingContext footballBettingContext)
        {
            this.footballBettingContext = footballBettingContext;
        }
        public IActionResult Index()
        {
            return this.RedirectToRoute("/Users/All");
        }
        [HttpGet]
        public IActionResult All()
        {
            List<User> users = footballBettingContext.Users.ToList();

            return this.View(users);
        }
    }
}
