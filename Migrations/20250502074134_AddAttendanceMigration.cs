using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Teachers_TeacherEmploymentNumber",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "TeacherEmploymentNumber",
                table: "Attendances",
                newName: "TeacherEmployeeNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_TeacherEmploymentNumber",
                table: "Attendances",
                newName: "IX_Attendances_TeacherEmployeeNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Teachers_TeacherEmployeeNumber",
                table: "Attendances",
                column: "TeacherEmployeeNumber",
                principalTable: "Teachers",
                principalColumn: "TeacherEmployeeNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Teachers_TeacherEmployeeNumber",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "TeacherEmployeeNumber",
                table: "Attendances",
                newName: "TeacherEmploymentNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_TeacherEmployeeNumber",
                table: "Attendances",
                newName: "IX_Attendances_TeacherEmploymentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Teachers_TeacherEmploymentNumber",
                table: "Attendances",
                column: "TeacherEmploymentNumber",
                principalTable: "Teachers",
                principalColumn: "TeacherEmployeeNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
