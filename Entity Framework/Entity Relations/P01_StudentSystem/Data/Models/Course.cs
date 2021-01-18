﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<StudentCourse> StudentsEnrolled { get; set; } = new HashSet<StudentCourse>();
        public virtual ICollection<Resource> Resources { get; set; } = new HashSet<Resource>();
        public virtual ICollection<Homework> HomeworkSubmissions { get; set; } = new HashSet<Homework>();
    }
}
