using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.Dto.Import
{
   public class ImportAnimalDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Type { get; set; }

        [Range(1,300)]
        public int Age { get; set; }

        [Required]
        public ImportPassportDto Passport { get; set; }
    }
}
