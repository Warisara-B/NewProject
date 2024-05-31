using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddTermFeeGroupLocalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TermFeeGroups",
                schema: "localization",
                columns: table => new
                {
                    TermFeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroups", x => new { x.TermFeeGroupId, x.Language });
                    table.ForeignKey(
                        name: "FK_TermFeeGroups_TermFeeGroups_TermFeeGroupId",
                        column: x => x.TermFeeGroupId,
                        principalTable: "TermFeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermFeeGroups",
                schema: "localization");
        }
    }
}
