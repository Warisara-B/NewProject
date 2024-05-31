using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyAcademicLevelRemoveCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AcademicLevels_Code",
                table: "AcademicLevels");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AcademicLevels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AcademicLevels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLevels_Code",
                table: "AcademicLevels",
                column: "Code",
                unique: true);
        }
    }
}
