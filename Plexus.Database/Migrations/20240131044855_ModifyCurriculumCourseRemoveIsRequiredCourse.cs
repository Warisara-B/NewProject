using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCurriculumCourseRemoveIsRequiredCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequiredCourse",
                table: "CurriculumCourses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredCourse",
                table: "CurriculumCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
