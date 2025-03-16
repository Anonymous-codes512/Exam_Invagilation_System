using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class Add_Teacher_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Teacher_TeacherId",
                table: "CheatingReports");

            migrationBuilder.DropIndex(
                name: "IX_CheatingReports_TeacherId",
                table: "CheatingReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "CheatingReports");

            migrationBuilder.RenameTable(
                name: "Teacher",
                newName: "Teachers");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmployeeNumber",
                table: "CheatingReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmployeeNumber",
                table: "Teachers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teachers_TeacherEmployeeNumber",
                table: "Teachers",
                column: "TeacherEmployeeNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_TeacherEmployeeNumber",
                table: "CheatingReports",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherEmployeeNumber",
                table: "Teachers",
                column: "TeacherEmployeeNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Teachers_TeacherEmployeeNumber",
                table: "CheatingReports",
                column: "TeacherEmployeeNumber",
                principalTable: "Teachers",
                principalColumn: "TeacherEmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Teachers_TeacherEmployeeNumber",
                table: "CheatingReports");

            migrationBuilder.DropIndex(
                name: "IX_CheatingReports_TeacherEmployeeNumber",
                table: "CheatingReports");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teachers_TeacherEmployeeNumber",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherEmployeeNumber",
                table: "Teachers");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teacher");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmployeeNumber",
                table: "CheatingReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "CheatingReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmployeeNumber",
                table: "Teacher",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_TeacherId",
                table: "CheatingReports",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Teacher_TeacherId",
                table: "CheatingReports",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
