
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Practice.Data.DataValidations.Course;
namespace Practice.Data.Models
{
   public class Course
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(CourseNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string  Description { get; set; }

        public ICollection<StudentInCourse> Students { get; set; } = new HashSet<StudentInCourse>();

        public ICollection<Homework> Homeworks { get; set; } = new HashSet<Homework>();
    }
}
