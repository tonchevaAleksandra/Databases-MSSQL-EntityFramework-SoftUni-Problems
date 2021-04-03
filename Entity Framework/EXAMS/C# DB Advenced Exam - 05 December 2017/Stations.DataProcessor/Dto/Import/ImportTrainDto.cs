using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stations.Models.Enums;

namespace Stations.DataProcessor.Dto.Import
{
    public class ImportTrainDto
    {
        [Required]
        [MaxLength(10)]
        public string TrainNumber { get; set; }

        [Range(0,2)]
        public TrainType? Type { get; set; }

        public List<ImportTrainSeatDto> Seats => new List<ImportTrainSeatDto>();
    }
}
