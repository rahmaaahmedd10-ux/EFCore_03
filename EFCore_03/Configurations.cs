using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore_03
{
    // Part 01:

    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FullName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(150);
            builder.Property(i => i.Bio).HasMaxLength(500);
        }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(300);

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.Price).HasPrecision(18, 2);
            builder.Property(c => c.Level).IsRequired().HasMaxLength(50);

            builder.HasOne(c => c.Instructor)
                   .WithMany(i => i.Courses)
                   .HasForeignKey(c => c.InstructorId);

            builder.HasOne(c => c.Category)
                   .WithMany(cat => cat.Courses)
                   .HasForeignKey(c => c.CategoryId);
        }
    }

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(150);
        }
    }

    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });
            builder.Property(e => e.Status).IsRequired().HasMaxLength(20);
            builder.Property(e => e.FinalGrade).HasPrecision(5, 2);

            builder.HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentId);

            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseId);
        }
    }
}
