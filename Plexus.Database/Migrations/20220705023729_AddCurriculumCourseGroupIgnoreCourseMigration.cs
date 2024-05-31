using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCurriculumCourseGroupIgnoreCourseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurriculumCourseGroupIgnoreCourses",
                columns: table => new
                {
                    CourseGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumCourseGroupIgnoreCourses", x => new { x.CourseGroupId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CurriculumCourseGroupIgnoreCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumCourseGroupIgnoreCourses_CurriculumCourseGroups_CourseGroupId",
                        column: x => x.CourseGroupId,
                        principalTable: "CurriculumCourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourseGroupIgnoreCourses_CourseId",
                table: "CurriculumCourseGroupIgnoreCourses",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumCourseGroupIgnoreCourses");
        }
    }
}
