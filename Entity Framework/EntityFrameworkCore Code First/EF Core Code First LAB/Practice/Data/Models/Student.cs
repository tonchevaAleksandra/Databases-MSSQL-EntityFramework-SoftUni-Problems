using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Practice.Data.DataValidations.Student;

namespace Practice.Data.Models
{
    //[Table("Pesho")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(StudentNamesMaxLength)]
        public string FirstName { get; set; }

        //[Column(TypeName ="nvarchar(100)")]
        [MaxLength(StudentNamesMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(StudentNamesMaxLength)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public bool HasScholarship { get; set; }

        public DateTime RegistrationDate { get; set; }

        public StudentType Type { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }//navigation property 

        public ICollection<StudentInCourse> Courses { get; set; } = new HashSet<StudentInCourse>();

        public ICollection<Homework> Homeworks { get; set; } = new HashSet<Homework>();

        [NotMapped] //EF do not assume this property as a column
        public string FullName
        {
            get
            {
                if (this.MiddleName == null)
                    return this.FirstName + " " + this.LastName;

                return this.FirstName + " " + this.MiddleName + " " + this.LastName;
            }
        }

        //[MaxLength(2*1024)]
        //public byte[] Image { get; set; }

        //[Column("EGN")]
        //[Column(TypeName ="char(10)")]

        //public string IdentificationNumber { get; set; }

    }
}
