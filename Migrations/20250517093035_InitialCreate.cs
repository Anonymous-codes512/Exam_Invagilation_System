using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam_Invagilation_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    TotalStrength = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.UniqueConstraint("AK_Rooms_RoomNumber", x => x.RoomNumber);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance = table.Column<int>(type: "int", nullable: false),
                    DuesClear = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.UniqueConstraint("AK_Students_RegistrationNumber", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                    table.UniqueConstraint("AK_Teachers_TeacherEmployeeNumber", x => x.TeacherEmployeeNumber);
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

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreRequisite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.UniqueConstraint("AK_Courses_CourseCode", x => x.CourseCode);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherEmployeeNumber",
                        column: x => x.TeacherEmployeeNumber,
                        principalTable: "Teachers",
                        principalColumn: "TeacherEmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaperSummaryCollections",
                columns: table => new
                {
                    PaperSummaryCollectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedPaper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roomnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "CheatingReports",
                columns: table => new
                {
                    CheatingReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnfairType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncidentDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheatingReports", x => x.CheatingReportId);
                    table.ForeignKey(
                        name: "FK_CheatingReports_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheatingReports_Rooms_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheatingReports_Students_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheatingReports_Teachers_TeacherEmployeeNumber",
                        column: x => x.TeacherEmployeeNumber,
                        principalTable: "Teachers",
                        principalColumn: "TeacherEmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Papers",
                columns: table => new
                {
                    PaperId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExamTerm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papers", x => x.PaperId);
                    table.ForeignKey(
                        name: "FK_Papers_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Papers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseAttendance = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => new { x.RegistrationNumber, x.CourseCode });
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        name: "FK_Attendances_Teachers_TeacherEmployeeNumber",
                        column: x => x.TeacherEmployeeNumber,
                        principalTable: "Teachers",
                        principalColumn: "TeacherEmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Duties",
                columns: table => new
                {
                    DutyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    PaperId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.DutyId);
                    table.ForeignKey(
                        name: "FK_Duties_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "PaperId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Duties_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Duties_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SittingArrangements",
                columns: table => new
                {
                    SittingArrangementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaperId = table.Column<int>(type: "int", nullable: false),
                    Seat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SittingArrangements", x => x.SittingArrangementId);
                    table.ForeignKey(
                        name: "FK_SittingArrangements_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "PaperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingArrangements_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingArrangements_Students_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Attendances_TeacherEmployeeNumber",
                table: "Attendances",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_CourseCode",
                table: "CheatingReports",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_RegistrationNumber",
                table: "CheatingReports",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_RoomNumber",
                table: "CheatingReports",
                column: "RoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CheatingReports_TeacherEmployeeNumber",
                table: "CheatingReports",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCode",
                table: "Courses",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherEmployeeNumber",
                table: "Courses",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_PaperId",
                table: "Duties",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_RoomId",
                table: "Duties",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_TeacherId",
                table: "Duties",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_CourseCode",
                table: "Papers",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_RoomId",
                table: "Papers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PaperSummaryCollections_TeacherEmployeeNumber",
                table: "PaperSummaryCollections",
                column: "TeacherEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber",
                table: "Rooms",
                column: "RoomNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SittingArrangements_PaperId",
                table: "SittingArrangements",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingArrangements_RegistrationNumber",
                table: "SittingArrangements",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SittingArrangements_RoomId",
                table: "SittingArrangements",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseCode",
                table: "StudentCourses",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RegistrationNumber",
                table: "Students",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherEmployeeNumber",
                table: "Teachers",
                column: "TeacherEmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "CheatingReports");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "PaperSummaryCollections");

            migrationBuilder.DropTable(
                name: "SittingArrangements");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Papers");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
