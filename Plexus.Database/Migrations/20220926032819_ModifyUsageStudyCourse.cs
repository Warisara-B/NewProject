using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyUsageStudyCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudyCourseId",
                table: "SectionSeatUsages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeatUsages_StudyCourseId",
                table: "SectionSeatUsages",
                column: "StudyCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeatUsages_StudyCourses_StudyCourseId",
                table: "SectionSeatUsages",
                column: "StudyCourseId",
                principalTable: "StudyCourses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeatUsages_StudyCourses_StudyCourseId",
                table: "SectionSeatUsages");

            migrationBuilder.DropIndex(
                name: "IX_SectionSeatUsages_StudyCourseId",
                table: "SectionSeatUsages");

            migrationBuilder.DropColumn(
                name: "StudyCourseId",
                table: "SectionSeatUsages");
        }
    }
}
