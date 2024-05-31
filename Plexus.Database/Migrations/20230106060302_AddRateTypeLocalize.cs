using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddRateTypeLocalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RateTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RateTypes",
                schema: "localization",
                columns: table => new
                {
                    RateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateTypes", x => new { x.RateTypeId, x.Language });
                    table.ForeignKey(
                        name: "FK_RateTypes_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RateTypes",
                schema: "localization");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RateTypes");
        }
    }
}
