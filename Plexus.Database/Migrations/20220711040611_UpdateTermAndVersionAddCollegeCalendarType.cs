using Microsoft.EntityFrameworkCore.Migrations;
using Plexus.Database.Enum.Academic;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class UpdateTermAndVersionAddCollegeCalendarType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CollegeCalendarType",
                table: "Terms",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: CollegeCalendarType.SEMESTER.ToString());

            migrationBuilder.AddColumn<string>(
                name: "CollegeCalendarType",
                table: "CurriculumVersions",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: CollegeCalendarType.SEMESTER.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollegeCalendarType",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "CollegeCalendarType",
                table: "CurriculumVersions");
        }
    }
}
