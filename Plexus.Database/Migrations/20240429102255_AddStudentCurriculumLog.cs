using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddStudentCurriculumLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StudentAcademicStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StudentCurriculumLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudyPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCurriculumLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCurriculumLogs_CurriculumVersions_CurriculumVersionId",
                        column: x => x.CurriculumVersionId,
                        principalTable: "CurriculumVersions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCurriculumLogs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCurriculumLogs_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCurriculumLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCurriculumLogs_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCurriculumLogs_CurriculumVersionId",
                table: "StudentCurriculumLogs",
                column: "CurriculumVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCurriculumLogs_DepartmentId",
                table: "StudentCurriculumLogs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCurriculumLogs_FacultyId",
                table: "StudentCurriculumLogs",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCurriculumLogs_StudentId",
                table: "StudentCurriculumLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCurriculumLogs_StudyPlanId",
                table: "StudentCurriculumLogs",
                column: "StudyPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCurriculumLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Students");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StudentAcademicStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
