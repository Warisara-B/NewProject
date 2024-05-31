using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddEquivalentCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquivalentCourses",
                columns: table => new
                {
                    CurriculumVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquivalenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquivalentCourses", x => new { x.CurriculumVersionId, x.CourseId, x.EquivalenceId });
                    table.ForeignKey(
                        name: "FK_EquivalentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquivalentCourses_Courses_EquivalenceId",
                        column: x => x.EquivalenceId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquivalentCourses_CurriculumVersions_CurriculumVersionId",
                        column: x => x.CurriculumVersionId,
                        principalTable: "CurriculumVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquivalentCourses_CourseId",
                table: "EquivalentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquivalentCourses_EquivalenceId",
                table: "EquivalentCourses",
                column: "EquivalenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquivalentCourses");
        }
    }
}
