using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static MyCoolCarSystem.Data.DataValidations.Model;

namespace MyCoolCarSystem.Data.Models
{
    public class Model
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxName)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MaxModification)]
        public string Modification { get; set; }

        public int Year { get; set; }

        public int MakeId { get; set; }

        public Make Make { get; set; }

        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();


    }
}
