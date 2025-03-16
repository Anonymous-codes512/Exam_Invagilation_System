//using System;
//using Exam_Invagilation_System.Models;
//using Exam_Invagilation_System.Entities;

//public class DBSeeder
//{
//    public static void SeedDatabase(AppDbContext context)
//    {
//        if (context.Students.Any() || context.Teachers.Any() || context.Rooms.Any() ||
//            context.Courses.Any() || context.Papers.Any())
//        {
//            return; // Database already seeded
//        }

//        // Seeding Students (20 records)
//        context.Students.AddRange(
//            new Student { StudentName = "Danish", FatherName = "Muhammad Ejaz Khan", Program = "Software Engineering", Semester = 7, Section = "B", Attendance = "80%", DuesClear = true },
//            new Student { StudentName = "Arbab Khursheed", FatherName = "Khursheed Hussain", Program = "Computer Science", Semester = 7, Section = "A", Attendance = "85%", DuesClear = true },
//            new Student { StudentName = "Hassan Raza", FatherName = "Raza Ahmed", Program = "Software Engineering", Semester = 6, Section = "C", Attendance = "75%", DuesClear = false },
//            new Student { StudentName = "Ayesha Khan", FatherName = "Imran Khan", Program = "Computer Science", Semester = 5, Section = "B", Attendance = "90%", DuesClear = true },
//            new Student { StudentName = "Sadia Anwar", FatherName = "Anwar Ali", Program = "Data Science", Semester = 4, Section = "A", Attendance = "92%", DuesClear = true },
//            new Student { StudentName = "Bilal Ahmed", FatherName = "Nadeem Ahmed", Program = "Artificial Intelligence", Semester = 3, Section = "C", Attendance = "78%", DuesClear = false },
//            new Student { StudentName = "Zain Malik", FatherName = "Faisal Malik", Program = "Software Engineering", Semester = 7, Section = "A", Attendance = "88%", DuesClear = true },
//            new Student { StudentName = "Rabia Iqbal", FatherName = "Iqbal Hussain", Program = "Cyber Security", Semester = 6, Section = "B", Attendance = "80%", DuesClear = false },
//            new Student { StudentName = "Shahid Ali", FatherName = "Ali Haider", Program = "Computer Science", Semester = 5, Section = "C", Attendance = "85%", DuesClear = true },
//            new Student { StudentName = "Fariha Noor", FatherName = "Noor Rehman", Program = "Software Engineering", Semester = 7, Section = "B", Attendance = "93%", DuesClear = true },
//            new Student { StudentName = "Hamza Tariq", FatherName = "Tariq Mehmood", Program = "Data Science", Semester = 4, Section = "A", Attendance = "77%", DuesClear = false },
//            new Student { StudentName = "Usman Javed", FatherName = "Javed Akhtar", Program = "Artificial Intelligence", Semester = 3, Section = "C", Attendance = "84%", DuesClear = true },
//            new Student { StudentName = "Sana Gul", FatherName = "Gul Khan", Program = "Cyber Security", Semester = 6, Section = "B", Attendance = "89%", DuesClear = true },
//            new Student { StudentName = "Tahir Mehmood", FatherName = "Mehmood Ali", Program = "Software Engineering", Semester = 5, Section = "A", Attendance = "82%", DuesClear = false },
//            new Student { StudentName = "Iqra Fatima", FatherName = "Fatima Sheikh", Program = "Computer Science", Semester = 4, Section = "C", Attendance = "91%", DuesClear = true },
//            new Student { StudentName = "Adnan Hussain", FatherName = "Hussain Raza", Program = "Data Science", Semester = 3, Section = "B", Attendance = "76%", DuesClear = false },
//            new Student { StudentName = "Noman Aslam", FatherName = "Aslam Khan", Program = "Cyber Security", Semester = 7, Section = "A", Attendance = "79%", DuesClear = true },
//            new Student { StudentName = "Aqsa Riaz", FatherName = "Riaz Ahmed", Program = "Artificial Intelligence", Semester = 6, Section = "C", Attendance = "86%", DuesClear = true },
//            new Student { StudentName = "Shafqat Ali", FatherName = "Ali Akbar", Program = "Software Engineering", Semester = 5, Section = "B", Attendance = "94%", DuesClear = true },
//            new Student { StudentName = "Farhan Malik", FatherName = "Malik Javed", Program = "Computer Science", Semester = 4, Section = "A", Attendance = "81%", DuesClear = false }
//        );

//        // Seeding Teachers (20 records)
//        context.Teachers.AddRange(
//            new Teacher { TeacherName = "Dr. Wasif Nisar", TeacherDesignation = "Professor", Department = "Software Engineering" },
//            new Teacher { TeacherName = "Dr. Imran Ahmad", TeacherDesignation = "Associate Professor", Department = "Computer Science" },
//            new Teacher { TeacherName = "Mr. Tariq Javed", TeacherDesignation = "Lecturer", Department = "Cyber Security" },
//            new Teacher { TeacherName = "Ms. Ayesha Raza", TeacherDesignation = "Assistant Professor", Department = "Artificial Intelligence" },
//            new Teacher { TeacherName = "Dr. Farhan Malik", TeacherDesignation = "Professor", Department = "Software Engineering" },
//            new Teacher { TeacherName = "Dr. Naveed Iqbal", TeacherDesignation = "Professor", Department = "Data Science" },
//            new Teacher { TeacherName = "Mr. Rashid Mahmood", TeacherDesignation = "Lecturer", Department = "Computer Science" },
//            new Teacher { TeacherName = "Ms. Sara Khan", TeacherDesignation = "Senior Lecturer", Department = "Software Engineering" },
//            new Teacher { TeacherName = "Dr. Kamran Zafar", TeacherDesignation = "Associate Professor", Department = "Artificial Intelligence" },
//            new Teacher { TeacherName = "Mr. Waleed Hassan", TeacherDesignation = "Assistant Professor", Department = "Cyber Security" },
//            new Teacher { TeacherName = "Dr. Uzair Ali", TeacherDesignation = "Professor", Department = "Software Engineering" },
//            new Teacher { TeacherName = "Ms. Fatima Tariq", TeacherDesignation = "Lecturer", Department = "Computer Science" },
//            new Teacher { TeacherName = "Dr. Salman Rehman", TeacherDesignation = "Professor", Department = "Cloud Computing" },
//            new Teacher { TeacherName = "Mr. Adeel Javed", TeacherDesignation = "Senior Lecturer", Department = "Networking" },
//            new Teacher { TeacherName = "Dr. Mehwish Akram", TeacherDesignation = "Associate Professor", Department = "Big Data" },
//            new Teacher { TeacherName = "Mr. Bilal Saeed", TeacherDesignation = "Lecturer", Department = "Cyber Security" },
//            new Teacher { TeacherName = "Dr. Yasir Shah", TeacherDesignation = "Professor", Department = "Artificial Intelligence" },
//            new Teacher { TeacherName = "Ms. Rabia Akhtar", TeacherDesignation = "Assistant Professor", Department = "Software Engineering" },
//            new Teacher { TeacherName = "Mr. Hamza Rauf", TeacherDesignation = "Lecturer", Department = "Machine Learning" },
//            new Teacher { TeacherName = "Dr. Shahzad Anwar", TeacherDesignation = "Professor", Department = "Computer Vision" }
//        );

//        // Seeding Rooms (10 records)
//        context.Rooms.AddRange(
//            new Room { RoomNo = "101", Description = "Lab 1", Columns = 5, Rows = 6, TotalStrength = 30 },
//            new Room { RoomNo = "102", Description = "Lecture Hall", Columns = 6, Rows = 8, TotalStrength = 48 },
//            new Room { RoomNo = "103", Description = "Seminar Room", Columns = 4, Rows = 5, TotalStrength = 20 },
//            new Room { RoomNo = "104", Description = "Examination Hall", Columns = 10, Rows = 10, TotalStrength = 100 },
//            new Room { RoomNo = "105", Description = "Research Lab", Columns = 3, Rows = 4, TotalStrength = 12 },
//            new Room { RoomNo = "106", Description = "Multimedia Room", Columns = 5, Rows = 7, TotalStrength = 35 },
//            new Room { RoomNo = "107", Description = "Physics Lab", Columns = 4, Rows = 6, TotalStrength = 24 },
//            new Room { RoomNo = "108", Description = "Computer Science Lab", Columns = 6, Rows = 5, TotalStrength = 30 },
//            new Room { RoomNo = "109", Description = "Mathematics Department Room", Columns = 4, Rows = 4, TotalStrength = 16 },
//            new Room { RoomNo = "110", Description = "Conference Room", Columns = 3, Rows = 5, TotalStrength = 15 }
//        );

//        // Seeding Courses (20 records)
//        context.Courses.AddRange(
//            new Course { CourseCode = "CS101", CourseName = "Introduction to Programming" },
//            new Course { CourseCode = "CS201", CourseName = "Data Structures" },
//            new Course { CourseCode = "CS301", CourseName = "Database Systems" },
//            new Course { CourseCode = "CS401", CourseName = "Artificial Intelligence" },
//            new Course { CourseCode = "CS501", CourseName = "Cyber Security" },
//            new Course { CourseCode = "SE101", CourseName = "Software Engineering Principles" },
//            new Course { CourseCode = "SE201", CourseName = "Object-Oriented Programming" },
//            new Course { CourseCode = "SE301", CourseName = "Software Architecture & Design" },
//            new Course { CourseCode = "SE401", CourseName = "Software Project Management" },
//            new Course { CourseCode = "IT101", CourseName = "Fundamentals of Information Technology" },
//            new Course { CourseCode = "IT201", CourseName = "Networking & Communication" },
//            new Course { CourseCode = "IT301", CourseName = "Cloud Computing" },
//            new Course { CourseCode = "IT401", CourseName = "Big Data Analytics" },
//            new Course { CourseCode = "EE101", CourseName = "Digital Logic Design" },
//            new Course { CourseCode = "EE201", CourseName = "Embedded Systems" }
//        );

//        // Seeding Papers (20 records)
//        context.Papers.AddRange(
//            new Paper { CourseCode = "CS101", RoomId = 1, Date = DateOnly.ParseExact("10-06-2025", "dd-MM-yyyy", null), TimeSlot = "09:00 AM - 12:00 PM", ExamTerm = "Midterm" },
//            new Paper { CourseCode = "CS201", RoomId = 2, Date = DateOnly.ParseExact("12-06-2025", "dd-MM-yyyy", null), TimeSlot = "01:00 PM - 04:00 PM", ExamTerm = "Final" },
//            new Paper { CourseCode = "CS301", RoomId = 3, Date = DateOnly.ParseExact("15-06-2025", "dd-MM-yyyy", null), TimeSlot = "09:00 AM - 12:00 PM", ExamTerm = "Midterm" },
//            new Paper { CourseCode = "CS401", RoomId = 4, Date = DateOnly.ParseExact("18-06-2025", "dd-MM-yyyy", null), TimeSlot = "01:00 PM - 04:00 PM", ExamTerm = "Final" },
//            new Paper { CourseCode = "CS501", RoomId = 5, Date = DateOnly.ParseExact("20-06-2025", "dd-MM-yyyy", null), TimeSlot = "09:00 AM - 12:00 PM", ExamTerm = "Midterm" },
//            new Paper { CourseCode = "SE101", RoomId = 6, Date = DateOnly.ParseExact("22-06-2025", "dd-MM-yyyy", null), TimeSlot = "01:00 PM - 04:00 PM", ExamTerm = "Final" },
//            new Paper { CourseCode = "SE201", RoomId = 7, Date = DateOnly.ParseExact("24-06-2025", "dd-MM-yyyy", null), TimeSlot = "09:00 AM - 12:00 PM", ExamTerm = "Midterm" },
//            new Paper { CourseCode = "SE301", RoomId = 8, Date = DateOnly.ParseExact("26-06-2025", "dd-MM-yyyy", null), TimeSlot = "01:00 PM - 04:00 PM", ExamTerm = "Final" },
//            new Paper { CourseCode = "IT101", RoomId = 9, Date = DateOnly.ParseExact("28-06-2025", "dd-MM-yyyy", null), TimeSlot = "09:00 AM - 12:00 PM", ExamTerm = "Midterm" },
//            new Paper { CourseCode = "IT201", RoomId = 10, Date = DateOnly.ParseExact("30-06-2025", "dd-MM-yyyy", null), TimeSlot = "01:00 PM - 04:00 PM", ExamTerm = "Final" }
//        );

//        // Seeding StudentCourses (20 records)x
//        context.StudentCourses.AddRange(
//            new StudentCourse { StudentId = 1, CourseCode = "CS101" },
//            new StudentCourse { StudentId = 2, CourseCode = "CS201" },
//            new StudentCourse { StudentId = 3, CourseCode = "CS301" },
//            new StudentCourse { StudentId = 4, CourseCode = "CS401" },
//            new StudentCourse { StudentId = 5, CourseCode = "SE101" },
//            new StudentCourse { StudentId = 6, CourseCode = "SE201" },
//            new StudentCourse { StudentId = 7, CourseCode = "CS501" },
//            new StudentCourse { StudentId = 8, CourseCode = "IT101" },
//            new StudentCourse { StudentId = 9, CourseCode = "IT201" },
//            new StudentCourse { StudentId = 10, CourseCode = "CS101" },
//            new StudentCourse { StudentId = 11, CourseCode = "CS201" },
//            new StudentCourse { StudentId = 12, CourseCode = "CS301" },
//            new StudentCourse { StudentId = 13, CourseCode = "CS401" },
//            new StudentCourse { StudentId = 14, CourseCode = "CS501" },
//            new StudentCourse { StudentId = 15, CourseCode = "SE101" },
//            new StudentCourse { StudentId = 16, CourseCode = "SE201" },
//            new StudentCourse { StudentId = 17, CourseCode = "IT101" },
//            new StudentCourse { StudentId = 18, CourseCode = "IT201" },
//            new StudentCourse { StudentId = 19, CourseCode = "CS101" },
//            new StudentCourse { StudentId = 20, CourseCode = "CS201" }
//        );

//        context.CheatingReports.AddRange(
//            new CheatingReport { CheatingReportId = 1, StudentId = 15, TeacherId = 2, RoomId = 1, CourseCode = "CS101", Date = DateOnly.Parse("2025-06-10"), TimeSlot = "09:00 AM - 12:00 PM", Description = "Caught using unauthorized notes." },
//            new CheatingReport { CheatingReportId = 2, StudentId = 12, TeacherId = 8, RoomId = 2, CourseCode = "CS201", Date = DateOnly.Parse("2025-06-12"), TimeSlot = "01:00 PM - 04:00 PM", Description = "Found communicating with another student." },
//            new CheatingReport { CheatingReportId = 3, StudentId = 3, TeacherId = 8, RoomId = 3, CourseCode = "CS301", Date = DateOnly.Parse("2025-06-15"), TimeSlot = "09:00 AM - 12:00 PM", Description = "Using mobile phone during the exam." }
//        );

//        context.SaveChanges();
//    }
//}
