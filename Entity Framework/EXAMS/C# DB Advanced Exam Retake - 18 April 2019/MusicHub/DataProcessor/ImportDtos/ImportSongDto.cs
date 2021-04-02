using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Song")]
   public class ImportSongDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        public string CreatedOn { get; set; }

        [Required]
        public string Genre { get; set; }

        public int? AlbumId { get; set; }

        public int WriterId { get; set; }

        [Range(typeof(decimal),"0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}
