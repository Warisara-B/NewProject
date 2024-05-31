using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCourseHourToCredits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherHour",
                table: "Courses",
                newName: "OtherCredit");

            migrationBuilder.RenameColumn(
                name: "LectureHour",
                table: "Courses",
                newName: "LectureCredit");

            migrationBuilder.RenameColumn(
                name: "LabHour",
                table: "Courses",
                newName: "LabCredit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherCredit",
                table: "Courses",
                newName: "OtherHour");

            migrationBuilder.RenameColumn(
                name: "LectureCredit",
                table: "Courses",
                newName: "LectureHour");

            migrationBuilder.RenameColumn(
                name: "LabCredit",
                table: "Courses",
                newName: "LabHour");
        }
    }
}
