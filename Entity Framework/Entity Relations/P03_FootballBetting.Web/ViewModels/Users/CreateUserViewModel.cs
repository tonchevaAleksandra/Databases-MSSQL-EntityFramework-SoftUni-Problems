using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P03_FootballBetting.Web.ViewModels.Users
{
    public class CreateUserViewModel
    {
        [Required]
        [MaxLength(50)]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MaxLength(26)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; set; }

    }
}
