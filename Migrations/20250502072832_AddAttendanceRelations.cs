using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherEmploymentNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaperId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "PaperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Rooms_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Teachers_TeacherEmploymentNumber",
                        column: x => x.TeacherEmploymentNumber,
                        principalTable: "Teachers",
                        principalColumn: "TeacherEmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_PaperId",
                table: "Attendances",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_RegistrationNumber",
                table: "Attendances",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_RoomNumber",
                table: "Attendances",
                column: "RoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TeacherEmploymentNumber",
                table: "Attendances",
                column: "TeacherEmploymentNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
