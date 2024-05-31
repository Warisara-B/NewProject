using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RemoveNameFromTerm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "localization",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "IsRegister",
                table: "Terms",
                newName: "IsRegistration");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                schema: "localization",
                table: "Faculties",
                newName: "FormalName");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                schema: "localization",
                table: "Departments",
                newName: "FormalName");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdvising",
                table: "Terms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdvising",
                table: "Terms");

            migrationBuilder.RenameColumn(
                name: "IsRegistration",
                table: "Terms",
                newName: "IsRegister");

            migrationBuilder.RenameColumn(
                name: "FormalName",
                schema: "localization",
                table: "Faculties",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "FormalName",
                schema: "localization",
                table: "Departments",
                newName: "ShortName");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "localization",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "localization",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
