using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Stations.Models.Enums;

namespace Stations.DataProcessor.Dto.Import
{
  public  class ImportTripDto
    {
        [Required]
        [MaxLength(10)]
        public string Train { get; set; }

        [Required]
        [MaxLength(50)]
        public string OriginStation { get; set; }

        [Required]
        [MaxLength(50)]
        public string DestinationStation { get; set; }

        [Required]
        public string DepartureTime { get; set; }

        [Required]
        public string ArrivalTime { get; set; }

        [Required]
        [Range(1,3)]
        public TripStatus Status { get; set; }

        public string TimeDifference { get; set; }
    }
}
