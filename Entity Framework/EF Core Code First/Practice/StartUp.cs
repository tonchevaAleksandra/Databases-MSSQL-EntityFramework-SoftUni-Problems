using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Data.Models;

namespace Practice
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using var db = new StudentDbContext();

            db.Database.Migrate();
            //db.Students.Add(new Student
            //{
            //    FirstName = "Aleksandra",
            //    LastName = "Toncheva",
            //    RegistrationDate = DateTime.UtcNow,
            //    Type = StudentType.Enrolled
            //});

            var town = new Town
            {
                Name = "Pleven"
            };
            //db.Towns.Add(town);
            //town.Students.Add(db.Students.First());


            var course = new Course
            {
                Name = "ASP.NET",
                Description = "ASP.NET MVC"
            };

            //db.Courses.Add(course);

            var studentId = db.Students.Select(st => st.Id).First();
            var courseId = db.Courses.Select(c => c.Id).First();
            //db.StudentsCourses.Add(new StudentInCourse
            //{
            //    StudentId = studentId,
            //    CourseId = courseId
            //});



            //db.StudentsCourses.Add(new StudentInCourse
            //{
            //    Student = new Student
            //    {
            //        FirstName = "Georgi",
            //        LastName = "Delchev",
            //        RegistrationDate = DateTime.UtcNow,
            //        Town = new Town
            //        {
            //            Name = "Stara Zagora"
            //        }
            //    },
            //    Course = new Course
            //    {
            //        Name = "C# OOP",
            //        Description = "Interfaces, Delegates"
            //    }
            //});
            //db.SaveChanges();

            //db.Homeworks.Add(new Homework
            //{
            //    Content = "Some content for this course",
            //    Score = 5.50,
            //    StudentId = 1,
            //    CourseId = 2,

            //}) ;

            //CoursesInformation(db); - Exception occured

            db.Students.ToList().ForEach(e => Console.WriteLine($"{e.FirstName} {e.LastName} {e.RegistrationDate} {e.Type} {e.TownId}"));


        }

        private static void CoursesInformation(StudentDbContext db)
        {
            var courses = db.Courses
                 .Select(c => new
                 {
                     c.Name,
                     TotalStudents = c.Students
                     .Where(st => st.Course.Homeworks.Average(h => h.Score) > 2)
                     .Count(),
                     Students = c
                     .Students
                     .Select(st => new
                     {
                         FullName = st.Student.FirstName + " " + st.Student.LastName,
                         Score = st.Student.Homeworks.Average(h => h.Score)
                     })
                     .ToList()
                 })
                 .ToList();


            Console.WriteLine(courses.Count());
        }
    }
}
