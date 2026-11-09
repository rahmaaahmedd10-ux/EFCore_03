using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCore_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.Migrate();

                Console.WriteLine("=== Part 02: Add Students & Enrollments ===");

                if (!db.Students.Any())
                {
                    var s1 = new Student { FullName = "Mena", Email = "mena@route.com", DateOfBirth = new DateTime(2001, 1, 1) };
                    var s2 = new Student { FullName = "Aya", Email = "aya@route.com", DateOfBirth = new DateTime(2002, 2, 2) };
                    var s3 = new Student { FullName = "Omar", Email = "omar@route.com", DateOfBirth = new DateTime(2000, 3, 3) };

                    db.Students.AddRange(s1, s2, s3);
                    db.SaveChanges();

                    db.Enrollments.AddRange(
                        new Enrollment { StudentId = s1.Id, CourseId = 1, EnrollmentDate = DateTime.Now, Status = "Active", Progress = 50 },
                        new Enrollment { StudentId = s2.Id, CourseId = 3, EnrollmentDate = DateTime.Now, Status = "Active", Progress = 80 },
                        new Enrollment { StudentId = s3.Id, CourseId = 4, EnrollmentDate = DateTime.Now, Status = "Active", Progress = 30 }
                    );
                    db.SaveChanges();
                    Console.WriteLine("Added Students and Enrollments Successfully.\n");
                }


                Console.WriteLine("=== Part 03: Queries ===");

                // 1.  Instructor - Category:
                Console.WriteLine("--- 1. Courses with Details ---");
                var coursesWithDetails = db.Courses.Include(c => c.Instructor).Include(c => c.Category).ToList();
                foreach (var c in coursesWithDetails)
                {
                    Console.WriteLine($"Course: {c.Title} | Instructor: {c.Instructor?.FullName} | Category: {c.Category?.Name}");
                }

                // 2. Published Courses:
                Console.WriteLine("\n--- 2. Published Courses ---");
                var publishedCourses = db.Courses.Where(c => c.IsPublished).OrderByDescending(c => c.PublishedDate).ToList();
                foreach (var c in publishedCourses)
                {
                    Console.WriteLine($"Course: {c.Title} | Date: {c.PublishedDate:yyyy-MM-dd}");
                }

                // 3. Title - Price Projection:
                Console.WriteLine("\n--- 3. Course Projections ---");
                var courseProjections = db.Courses.Select(c => new { c.Title, c.Price }).ToList();
                foreach (var cp in courseProjections)
                {
                    Console.WriteLine($"Title: {cp.Title} | Price: {cp.Price} EGP");
                }

                // 4. Average Price :
                Console.WriteLine("\n--- 4. Average Price per Category ---");
                var avgPricePerCategory = db.Courses.GroupBy(c => c.Category.Name)
                                            .Select(g => new { CategoryName = g.Key, AvgPrice = g.Average(c => c.Price) }).ToList();
                foreach (var item in avgPricePerCategory)
                {
                    Console.WriteLine($"Category: {item.CategoryName} | Avg Price: {item.AvgPrice:F2} EGP");
                }

                // 5. Enrollments
                Console.WriteLine("\n--- 5. Student Enrollments ---");
                var studentEnrollments = db.Enrollments.Include(e => e.Student).Include(e => e.Course).ToList();
                foreach (var e in studentEnrollments)
                {
                    Console.WriteLine($"Student: {e.Student?.FullName} | Course: {e.Course?.Title} | Status: {e.Status}");
                }

                // 6. Update:
                Console.WriteLine("\n--- 6. Update Price ---");
                var courseToUpdate = db.Courses.FirstOrDefault(c => c.Id == 1);
                if (courseToUpdate != null)
                {
                    courseToUpdate.Price = 120m;
                    db.SaveChanges();
                    Console.WriteLine($"Updated Course 1 Price to: {courseToUpdate.Price} EGP");
                }

                // 7. Delete:
                Console.WriteLine("\n--- 7. Delete Course ---");
                var courseToDelete = db.Courses.FirstOrDefault(c => c.Id == 4);
                if (courseToDelete != null)
                {
                    db.Courses.Remove(courseToDelete);
                    db.SaveChanges();
                    Console.WriteLine("Course 4 deleted.");
                }
            }
        }
    }
}