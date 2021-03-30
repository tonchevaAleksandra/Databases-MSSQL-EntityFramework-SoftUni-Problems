using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Projections = new HashSet<Projection>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public MovieGenre Genre { get; set; }

        public TimeSpan Duration { get; set; }

        public double Rating { get; set; }

        [Required]
        [MaxLength(20)]
        public string Director { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

    }
}
