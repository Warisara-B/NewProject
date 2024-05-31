using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddAcademicSpecialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicSpecializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentAcademicSpecializationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSpecializations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicSpecializations_AcademicSpecializations_ParentAcademicSpecializationId",
                        column: x => x.ParentAcademicSpecializationId,
                        principalTable: "AcademicSpecializations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcademicSpecializations",
                schema: "localization",
                columns: table => new
                {
                    AcademicSpecializationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSpecializations", x => new { x.AcademicSpecializationId, x.Language });
                    table.ForeignKey(
                        name: "FK_AcademicSpecializations_AcademicSpecializations_AcademicSpecializationId",
                        column: x => x.AcademicSpecializationId,
                        principalTable: "AcademicSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationCourses",
                columns: table => new
                {
                    AcademicSpecializationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRequiredCourse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationCourses", x => new { x.AcademicSpecializationId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_SpecializationCourses_AcademicSpecializations_AcademicSpecializationId",
                        column: x => x.AcademicSpecializationId,
                        principalTable: "AcademicSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationCourses_Grades_RequiredGradeId",
                        column: x => x.RequiredGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourseGroups_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                column: "AcademicSpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSpecializations_ParentAcademicSpecializationId",
                table: "AcademicSpecializations",
                column: "ParentAcademicSpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationCourses_CourseId",
                table: "SpecializationCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationCourses_RequiredGradeId",
                table: "SpecializationCourses",
                column: "RequiredGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumCourseGroups_AcademicSpecializations_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                column: "AcademicSpecializationId",
                principalTable: "AcademicSpecializations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumCourseGroups_AcademicSpecializations_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropTable(
                name: "AcademicSpecializations",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "SpecializationCourses");

            migrationBuilder.DropTable(
                name: "AcademicSpecializations");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumCourseGroups_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropColumn(
                name: "AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");
        }
    }
}
