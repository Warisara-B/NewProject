using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddSeatIdToStudyCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionSeatId",
                table: "StudyCourses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudyCourses_SectionSeatId",
                table: "StudyCourses",
                column: "SectionSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses",
                column: "SectionSeatId",
                principalTable: "SectionSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudyCourses_SectionSeatId",
                table: "StudyCourses");

            migrationBuilder.DropColumn(
                name: "SectionSeatId",
                table: "StudyCourses");
        }
    }
}
