using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentScholarship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "StudentScholarshipUsages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "StudentScholarshipUsages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendContract",
                table: "StudentScholarships",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "StudentScholarshipUsages");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "StudentScholarshipUsages");

            migrationBuilder.DropColumn(
                name: "IsSendContract",
                table: "StudentScholarships");
        }
    }
}
