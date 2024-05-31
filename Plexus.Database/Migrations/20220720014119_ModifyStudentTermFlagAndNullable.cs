using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentTermFlagAndNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionType",
                table: "StudentTerms");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPAX",
                table: "StudentTerms",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayable",
                table: "StudentTerms");

            migrationBuilder.DropColumn(
                name: "IsRegistrable",
                table: "StudentTerms");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPAX",
                table: "StudentTerms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermissionType",
                table: "StudentTerms",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }
    }
}
