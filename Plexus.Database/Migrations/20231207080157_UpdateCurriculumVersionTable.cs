using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class UpdateCurriculumVersionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "CurriculumCourseGroups");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CurriculumCourseGroups");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "CurriculumCourseGroups",
                newName: "Sequence");

            migrationBuilder.AddColumn<string>(
                name: "MigrationReference",
                table: "CurriculumCourseGroups",
                type: "nvarchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MigrationReference",
                table: "CurriculumCourseGroups");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "CurriculumCourseGroups",
                newName: "Level");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "CurriculumCourseGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CurriculumCourseGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
