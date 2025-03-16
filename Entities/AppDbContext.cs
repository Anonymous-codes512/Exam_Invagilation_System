using Exam_Invagilation_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CheatingReport> CheatingReports { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Room> Rooms { get; set; }

        //public DbSet<Attendance> Attendances { get; set; }
        //public DbSet<Duty> Duties { get; set; }
        //public DbSet<Paper> Papers { get; set; }
        //public DbSet<SittingArrangement> SittingArrangements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
            // Define RegistrationNumber as an alternate key
            modelBuilder.Entity<Student>().HasAlternateKey(s => s.RegistrationNumber);
            // Ensure RegistrationNumber is unique
            modelBuilder.Entity<Student>().HasIndex(s => s.RegistrationNumber).IsUnique();

            modelBuilder.Entity<Course>()
        .HasKey(c => c.CourseId);  // Primary Key

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();  // Secondary Unique Key

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseCode)
                .HasPrincipalKey(c => c.CourseCode);


            //Define StudentCourse composite key
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.RegistrationNumber, sc.CourseCode });
            // Student - StudentCourse relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasPrincipalKey(s => s.RegistrationNumber) 
                .HasForeignKey(sc => sc.RegistrationNumber);

            // Course - StudentCourse relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseCode);

            // ✅ Fixed: Student - CheatingReport relationship (Using RegistrationNumber as FK)
            modelBuilder.Entity<CheatingReport>().HasOne(cr => cr.Student).WithMany().HasPrincipalKey(s => s.RegistrationNumber).HasForeignKey(cr => cr.RegistrationNumber);

            modelBuilder.Entity<Teacher>().HasKey(t => t.TeacherId);

            // Define TeacherEmployeeNumber as an alternate key
            modelBuilder.Entity<Teacher>().HasAlternateKey(t => t.TeacherEmployeeNumber);

            // Ensure TeacherEmployeeNumber is unique
            modelBuilder.Entity<Teacher>().HasIndex(t => t.TeacherEmployeeNumber).IsUnique();

            // ✅ Fixed: Teacher - CheatingReport relationship (Using TeacherEmployeeNumber as FK)
            modelBuilder.Entity<CheatingReport>().HasOne(cr => cr.Teacher).WithMany().HasPrincipalKey(t => t.TeacherEmployeeNumber).HasForeignKey(cr => cr.TeacherEmployeeNumber);

            modelBuilder.Entity<Room>().HasKey(r => r.RoomId);
            // Define RoomNumber as an alternate key
            modelBuilder.Entity<Room>().HasAlternateKey(r => r.RoomNumber);
            // Ensure RoomNumber is unique
            modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique();

        }
    }
}
