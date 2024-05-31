using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RemoveAcademicCalendarStudentAndInstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicCalendarInstructors");

            migrationBuilder.DropTable(
                name: "AcademicCalendarStudents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicCalendarInstructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicCalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicCalendarInstructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicCalendarInstructors_AcademicCalendars_AcademicCalendarId",
                        column: x => x.AcademicCalendarId,
                        principalTable: "AcademicCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicCalendarInstructors_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicCalendarStudents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicCalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicCalendarStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicCalendarStudents_AcademicCalendars_AcademicCalendarId",
                        column: x => x.AcademicCalendarId,
                        principalTable: "AcademicCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicCalendarStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarInstructors_AcademicCalendarId",
                table: "AcademicCalendarInstructors",
                column: "AcademicCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarInstructors_EmployeeId",
                table: "AcademicCalendarInstructors",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarStudents_AcademicCalendarId",
                table: "AcademicCalendarStudents",
                column: "AcademicCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarStudents_StudentId",
                table: "AcademicCalendarStudents",
                column: "StudentId");
        }
    }
}
