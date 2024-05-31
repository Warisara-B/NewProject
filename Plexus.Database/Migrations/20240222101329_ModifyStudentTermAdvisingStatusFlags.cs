using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentTermAdvisingStatusFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayable",
                table: "StudentTerms");

            migrationBuilder.DropColumn(
                name: "IsRegistrable",
                table: "StudentTerms");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentTerms",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AdvisingSlots",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentTerms");

            migrationBuilder.AddColumn<bool>(
                name: "IsPayable",
                table: "StudentTerms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistrable",
                table: "StudentTerms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdvisingSlots",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
