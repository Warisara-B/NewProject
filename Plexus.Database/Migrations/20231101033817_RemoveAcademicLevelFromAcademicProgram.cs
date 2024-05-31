using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RemoveAcademicLevelFromAcademicProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPrograms_AcademicLevels_AcademicLevelId",
                table: "AcademicPrograms");

            migrationBuilder.DropIndex(
                name: "IX_AcademicPrograms_AcademicLevelId",
                table: "AcademicPrograms");

            migrationBuilder.DropColumn(
                name: "AcademicLevelId",
                table: "AcademicPrograms");

            migrationBuilder.AddColumn<string>(
                name: "FormalName",
                schema: "localization",
                table: "AcademicPrograms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormalName",
                table: "AcademicPrograms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormalName",
                schema: "localization",
                table: "AcademicPrograms");

            migrationBuilder.DropColumn(
                name: "FormalName",
                table: "AcademicPrograms");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicLevelId",
                table: "AcademicPrograms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPrograms_AcademicLevelId",
                table: "AcademicPrograms",
                column: "AcademicLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPrograms_AcademicLevels_AcademicLevelId",
                table: "AcademicPrograms",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
