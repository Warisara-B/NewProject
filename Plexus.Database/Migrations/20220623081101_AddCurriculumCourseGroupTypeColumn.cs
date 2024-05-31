using Microsoft.EntityFrameworkCore.Migrations;
using Plexus.Database.Enum.Academic.Curriculum;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCurriculumCourseGroupTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CurriculumCourseGroups",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "REQUIRED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "CurriculumCourseGroups");
        }
    }
}
