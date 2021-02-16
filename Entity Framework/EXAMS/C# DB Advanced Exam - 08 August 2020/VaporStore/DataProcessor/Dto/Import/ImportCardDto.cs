using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Import
{
   public class ImportCardDto
    {
        [Required]
        [RegularExpression(@"^(\d){4} (\d){4} (\d){4} (\d){4}$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^(\d){3}$")]
        [JsonProperty("CVC")]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
