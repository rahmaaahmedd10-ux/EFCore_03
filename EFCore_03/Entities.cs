using System;
using System.Collections.Generic;

namespace EFCore_03
{
    // Part 01:

    public class Instructor
    {
        public int Id { get; set; }
        public string ?FullName { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public DateTime JoinedDate { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }

    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }

    public class Course
    {
        public int Id { get; set; }
        public string ? Title { get; set; }
        public string ?Description { get; set; }
        public decimal Price { get; set; }
        public string ? Level { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public int CategoryId { get; set; }
        public Category ?Category { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class Student
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student ? Student { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int Progress { get; set; }
        public string ?Status { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}