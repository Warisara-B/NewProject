using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class FixedAcademicSpecializationLocale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumCourseGroups_AcademicSpecializations_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumCourseGroups_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropColumn(
                name: "AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourseGroups_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                column: "AcademicSpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumCourseGroups_AcademicSpecializations_AcademicSpecializationId",
                schema: "localization",
                table: "CurriculumCourseGroups",
                column: "AcademicSpecializationId",
                principalTable: "AcademicSpecializations",
                principalColumn: "Id");
        }
    }
}
