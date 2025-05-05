using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseAttendance",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseAttendance",
                table: "Courses");
        }
    }
}
