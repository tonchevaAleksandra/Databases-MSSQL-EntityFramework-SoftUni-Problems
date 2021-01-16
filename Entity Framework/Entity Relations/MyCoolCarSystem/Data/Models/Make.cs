using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static MyCoolCarSystem.Data.DataValidations.Make;

namespace MyCoolCarSystem.Data.Models
{
    public class Make
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxName)]
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; } = new HashSet<Model>();

       


    }
}
