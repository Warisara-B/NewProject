using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RenameEquivalentCourseColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquivalentCourses_Courses_EquivalenceId",
                table: "EquivalentCourses");

            migrationBuilder.RenameColumn(
                name: "EquivalenceId",
                table: "EquivalentCourses",
                newName: "EquivalenceCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_EquivalentCourses_EquivalenceId",
                table: "EquivalentCourses",
                newName: "IX_EquivalentCourses_EquivalenceCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquivalentCourses_Courses_EquivalenceCourseId",
                table: "EquivalentCourses",
                column: "EquivalenceCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquivalentCourses_Courses_EquivalenceCourseId",
                table: "EquivalentCourses");

            migrationBuilder.RenameColumn(
                name: "EquivalenceCourseId",
                table: "EquivalentCourses",
                newName: "EquivalenceId");

            migrationBuilder.RenameIndex(
                name: "IX_EquivalentCourses_EquivalenceCourseId",
                table: "EquivalentCourses",
                newName: "IX_EquivalentCourses_EquivalenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquivalentCourses_Courses_EquivalenceId",
                table: "EquivalentCourses",
                column: "EquivalenceId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
