using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class StudyCourseNullableVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionSeatId",
                table: "StudyCourses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses",
                column: "SectionSeatId",
                principalTable: "SectionSeats",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionSeatId",
                table: "StudyCourses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyCourses_SectionSeats_SectionSeatId",
                table: "StudyCourses",
                column: "SectionSeatId",
                principalTable: "SectionSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
