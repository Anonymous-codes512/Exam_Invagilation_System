using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentCourseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseAttendance",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "CourseAttendance",
                table: "StudentCourses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseAttendance",
                table: "StudentCourses");

            migrationBuilder.AddColumn<string>(
                name: "CourseAttendance",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
