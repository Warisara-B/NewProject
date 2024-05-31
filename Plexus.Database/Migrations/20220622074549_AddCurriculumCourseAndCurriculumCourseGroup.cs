using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCurriculumCourseAndCurriculumCourseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurriculumCourseGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentCourseGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumCourseGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurriculumCourseGroups_CurriculumCourseGroups_ParentCourseGroupId",
                        column: x => x.ParentCourseGroupId,
                        principalTable: "CurriculumCourseGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurriculumCourseGroups_CurriculumVersions_CurriculumVersionId",
                        column: x => x.CurriculumVersionId,
                        principalTable: "CurriculumVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumCourses",
                columns: table => new
                {
                    CourseGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRequiredCourse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumCourses", x => new { x.CourseGroupId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CurriculumCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumCourses_CurriculumCourseGroups_CourseGroupId",
                        column: x => x.CourseGroupId,
                        principalTable: "CurriculumCourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumCourses_Grades_RequiredGradeId",
                        column: x => x.RequiredGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourseGroups_CurriculumVersionId",
                table: "CurriculumCourseGroups",
                column: "CurriculumVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourseGroups_ParentCourseGroupId",
                table: "CurriculumCourseGroups",
                column: "ParentCourseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourses_CourseId",
                table: "CurriculumCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourses_RequiredGradeId",
                table: "CurriculumCourses",
                column: "RequiredGradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumCourses");

            migrationBuilder.DropTable(
                name: "CurriculumCourseGroups");
        }
    }
}
