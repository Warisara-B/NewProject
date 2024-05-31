using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifySectionSeatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SectionSeatUsages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "SectionSeats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeats_SectionId",
                table: "SectionSeats",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeats_Sections_SectionId",
                table: "SectionSeats",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeats_Sections_SectionId",
                table: "SectionSeats");

            migrationBuilder.DropIndex(
                name: "IX_SectionSeats_SectionId",
                table: "SectionSeats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SectionSeatUsages");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "SectionSeats");
        }
    }
}
