using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCourseGroupRemoveAbbreviation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "localization",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "CurriculumCourseGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "localization",
                table: "CurriculumCourseGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "CurriculumCourseGroups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
