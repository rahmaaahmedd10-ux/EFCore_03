using System;
using Microsoft.EntityFrameworkCore;

namespace EFCore_03
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EFCore03Db;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //API Configurations
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programming", Description = "Programming Basics", ParentCategoryId = null },
                new Category { Id = 2, Name = "Cyber Security", Description = "Security Fundamentals", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "Information Technology", Description = "IT & Infrastructure", ParentCategoryId = 1 }
            );

            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, FullName = "Ahmed Ayman", Email = "ahmed@route.com", Bio = "Programming Instructor", JoinedDate = new System.DateTime(2020, 1, 1) },
                new Instructor { Id = 2, FullName = "Hossam", Email = "hossam@route.com", Bio = "Cyber Security Expert", JoinedDate = new System.DateTime(2021, 5, 10) },
                new Instructor { Id = 3, FullName = "Rahma Ahmed", Email = "rahma@route.com", Bio = "IT Specialist", JoinedDate = new System.DateTime(2019, 3, 15) }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "AI", Description = "Intro to AI", Price = 100m, Level = "Beginner", IsPublished = true, PublishedDate = new System.DateTime(2022, 1, 1), CategoryId = 1, InstructorId = 1 },
                new Course { Id = 2, Title = ".NET", Description = ".NET Core Course", Price = 200m, Level = "Intermediate", IsPublished = true, PublishedDate = new System.DateTime(2022, 6, 1), CategoryId = 1, InstructorId = 1 },
                new Course { Id = 3, Title = "Cyber Security", Description = "Security Basics", Price = 150m, Level = "Beginner", IsPublished = true, PublishedDate = new System.DateTime(2023, 2, 1), CategoryId = 2, InstructorId = 2 },
                new Course { Id = 4, Title = "C# Basic", Description = "C# Fundamentals", Price = 80m, Level = "Beginner", IsPublished = true, PublishedDate = new System.DateTime(2023, 5, 1), CategoryId = 1, InstructorId = 3 }
            );
        }
    }
}