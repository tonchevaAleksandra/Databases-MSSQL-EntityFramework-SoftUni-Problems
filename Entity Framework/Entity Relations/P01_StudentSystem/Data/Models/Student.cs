﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public DateTime? Birthday { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<StudentCourse> CourseEnrollments { get; set; } = new HashSet<StudentCourse>();
        public virtual ICollection<Homework> HomeworkSubmissions { get; set; } = new HashSet<Homework>();
    }

}
