using System.Collections.Generic;
using System.Reflection.Emit;
using Exam_Invagilation_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CheatingReport> CheatingReports { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<SittingArrangement> SittingArrangements { get; set; }
        public DbSet<PaperSummaryCollection> PaperSummaryCollections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Define Student Primary Key
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
            modelBuilder.Entity<Student>().HasAlternateKey(s => s.RegistrationNumber);
            modelBuilder.Entity<Student>().HasIndex(s => s.RegistrationNumber).IsUnique();

            // ✅ Define Teacher Primary Key
            modelBuilder.Entity<Teacher>().HasKey(t => t.TeacherId);
            modelBuilder.Entity<Teacher>().HasAlternateKey(t => t.TeacherEmployeeNumber);
            modelBuilder.Entity<Teacher>().HasIndex(t => t.TeacherEmployeeNumber).IsUnique();

            // ✅ Define Course Primary Key
            modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
            modelBuilder.Entity<Course>().HasIndex(c => c.CourseCode).IsUnique(); // Unique Course Code

            // ✅ One Teacher Teaches Many Courses
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses) // One teacher teaches multiple courses
                .HasForeignKey(c => c.TeacherEmployeeNumber) // FK in Course
                .HasPrincipalKey(t => t.TeacherEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion

            // ✅ Define StudentCourse Composite Key (Many-to-Many: Student - Course)
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.RegistrationNumber, sc.CourseCode });

            // ✅ StudentCourse - Student Relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasPrincipalKey(s => s.RegistrationNumber)
                .HasForeignKey(sc => sc.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade); // If a student is deleted, remove enrollments

            // ✅ StudentCourse - Course Relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasPrincipalKey(c => c.CourseCode)
                .HasForeignKey(sc => sc.CourseCode)
                .OnDelete(DeleteBehavior.Cascade); // If a course is deleted, remove enrollments

            // ✅ Define Room Primary Key
            modelBuilder.Entity<Room>().HasKey(r => r.RoomId);
            modelBuilder.Entity<Room>().HasAlternateKey(r => r.RoomNumber);
            modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique();

            // ✅ Define CheatingReport - Student Relationship (Using RegistrationNumber as FK)
            modelBuilder.Entity<CheatingReport>()
                .HasOne(cr => cr.Student)
                .WithMany()
                .HasPrincipalKey(s => s.RegistrationNumber)
                .HasForeignKey(cr => cr.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Define CheatingReport - Teacher Relationship (Using TeacherEmployeeNumber as FK)
            modelBuilder.Entity<CheatingReport>()
                .HasOne(cr => cr.Teacher)
                .WithMany()
                .HasPrincipalKey(t => t.TeacherEmployeeNumber)
                .HasForeignKey(cr => cr.TeacherEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CheatingReport>()
                .HasOne(cr => cr.Course)
                .WithMany()
               .HasForeignKey(cr => cr.CourseCode) // Foreign key on CheatingReport
               .HasPrincipalKey(c => c.CourseCode) // Principal key on Course
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CheatingReport>()
               .HasOne(cr => cr.Room)
               .WithMany()
               .HasForeignKey(cr => cr.RoomNumber) // Foreign key on CheatingReport
               .HasPrincipalKey(r => r.RoomNumber) // Principal key on Room
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Paper>().HasKey(p => p.PaperId);
            // ✅ Define Paper - Course Relationship
            modelBuilder.Entity<Paper>()
                .HasOne(p => p.Course)
                .WithMany()
                .HasForeignKey(p => p.CourseCode)
                .HasPrincipalKey(c => c.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Define Paper - Room Relationship (Using RoomId as FK)
            modelBuilder.Entity<Paper>()
                .HasOne(p => p.Room)
                .WithMany()
                .HasForeignKey(p => p.RoomId)
                .HasPrincipalKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Duty>().HasKey(d => d.DutyId);
            modelBuilder.Entity<Duty>()
                .HasOne(d => d.Teacher)
                .WithMany()
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Duty>()
                .HasOne(d => d.Room)
                .WithMany()
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SittingArrangement>()
                .HasOne(sa => sa.Paper)
                .WithMany()
                .HasForeignKey(sa => sa.PaperId)
                .OnDelete(DeleteBehavior.Restrict);

            // SittingArrangement - Room Relationship
            modelBuilder.Entity<SittingArrangement>()
                .HasOne(sa => sa.Room)
                .WithMany()
                .HasForeignKey(sa => sa.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // SittingArrangement - Student Relationship
            modelBuilder.Entity<SittingArrangement>()
                .HasOne(sa => sa.Student)
                .WithMany()
                .HasPrincipalKey(s => s.RegistrationNumber)
                .HasForeignKey(sc => sc.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Attendance - Student Relationship (RegistrationNumber as FK)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasPrincipalKey(s => s.RegistrationNumber)
                .HasForeignKey(a => a.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Attendance - Teacher Relationship (TeacherEmployeeNumber as FK)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Teacher)
                .WithMany()
                .HasPrincipalKey(t => t.TeacherEmployeeNumber)
                .HasForeignKey(a => a.TeacherEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Attendance - Room Relationship (RoomNumber as FK)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Room)
                .WithMany()
                .HasPrincipalKey(r => r.RoomNumber)
                .HasForeignKey(a => a.RoomNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Attendance - Paper Relationship
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Paper)
                .WithMany()
                .HasForeignKey(a => a.PaperId)
                .HasPrincipalKey(p => p.PaperId)
                .OnDelete(DeleteBehavior.Restrict);

            // New PaperSummaryCollection - Teacher Relationship
            modelBuilder.Entity<PaperSummaryCollection>()
                .HasOne(psc => psc.Teacher)
                .WithMany()
                .HasForeignKey(psc => psc.TeacherEmployeeNumber)
                .HasPrincipalKey(t => t.TeacherEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict deletion of teacher

        }
    }
}