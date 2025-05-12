using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperSummaryCollectionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "CheatingReports");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "CheatingReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IncidentDetails",
                table: "CheatingReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherDetails",
                table: "CheatingReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoomNumber",
                table: "CheatingReports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnfairType",
                table: "CheatingReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaperSummaryCollections",
                columns: table => new
                {
                    PaperSummaryCollectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedPaper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPapers = table.Column<int>(type: "int", nullable: false),
                    NumberOfPapers = table.Column<int>(type: "int", nullable: false),
                    CollectorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperSummaryCollections", x => x.PaperSummaryCollectionId);
                    table.ForeignKey(
                        name: "FK_PaperSummaryCollections_Teachers_TeacherEmployeeNumber",
                        column: x => x.TeacherEmployeeNumber,
                        principalTable: "Teachers",
                        principalColumn: "TeacherEmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_CourseCode",
                table: "CheatingReports",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_RoomNumber",
                table: "CheatingReports",
                column: "RoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PaperSummaryCollections_TeacherEmployeeNumber",
                table: "PaperSummaryCollections",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Courses_CourseCode",
                table: "CheatingReports",
                column: "CourseCode",
                principalTable: "Courses",
                principalColumn: "CourseCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Rooms_RoomNumber",
                table: "CheatingReports",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Courses_CourseCode",
                table: "CheatingReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Rooms_RoomNumber",
                table: "CheatingReports");

            migrationBuilder.DropTable(
                name: "PaperSummaryCollections");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CheatingReports_CourseCode",
                table: "CheatingReports");

            migrationBuilder.DropIndex(
                name: "IX_CheatingReports_RoomNumber",
                table: "CheatingReports");

            migrationBuilder.DropColumn(
                name: "IncidentDetails",
                table: "CheatingReports");

            migrationBuilder.DropColumn(
                name: "OtherDetails",
                table: "CheatingReports");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "CheatingReports");

            migrationBuilder.DropColumn(
                name: "UnfairType",
                table: "CheatingReports");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "CheatingReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "CheatingReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
