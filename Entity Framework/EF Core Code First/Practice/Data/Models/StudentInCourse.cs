using System;
using System.Collections.Generic;
using System.Text;

namespace Practice.Data.Models
{
    public class StudentInCourse
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
