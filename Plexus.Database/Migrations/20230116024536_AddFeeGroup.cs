using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddFeeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeeGroupId",
                table: "FeeTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeeGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeTypes_FeeGroupId",
                table: "FeeTypes",
                column: "FeeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTypes_FeeGroups_FeeGroupId",
                table: "FeeTypes",
                column: "FeeGroupId",
                principalTable: "FeeGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeTypes_FeeGroups_FeeGroupId",
                table: "FeeTypes");

            migrationBuilder.DropTable(
                name: "FeeGroups");

            migrationBuilder.DropIndex(
                name: "IX_FeeTypes_FeeGroupId",
                table: "FeeTypes");

            migrationBuilder.DropColumn(
                name: "FeeGroupId",
                table: "FeeTypes");
        }
    }
}
