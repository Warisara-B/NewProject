using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class FixStudentLocalizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students",
                columns: new[] { "StudentId", "Language" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students",
                column: "StudentId");
        }
    }
}
