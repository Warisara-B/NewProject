using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCoursePrerequisiteStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumCoursePrerequisites_Courses_CourseId",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CurriculumCoursePrerequisites",
                newName: "CoursePrerequisiteId");

            migrationBuilder.RenameIndex(
                name: "IX_CurriculumCoursePrerequisites_CourseId",
                table: "CurriculumCoursePrerequisites",
                newName: "IX_CurriculumCoursePrerequisites_CoursePrerequisiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumCoursePrerequisites_CoursePrerequisites_CoursePrerequisiteId",
                table: "CurriculumCoursePrerequisites",
                column: "CoursePrerequisiteId",
                principalTable: "CoursePrerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumCoursePrerequisites_CoursePrerequisites_CoursePrerequisiteId",
                table: "CurriculumCoursePrerequisites");

            migrationBuilder.RenameColumn(
                name: "CoursePrerequisiteId",
                table: "CurriculumCoursePrerequisites",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CurriculumCoursePrerequisites_CoursePrerequisiteId",
                table: "CurriculumCoursePrerequisites",
                newName: "IX_CurriculumCoursePrerequisites_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "CurriculumCoursePrerequisites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CurriculumCoursePrerequisites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CurriculumCoursePrerequisites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "CurriculumCoursePrerequisites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumCoursePrerequisites_Courses_CourseId",
                table: "CurriculumCoursePrerequisites",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
