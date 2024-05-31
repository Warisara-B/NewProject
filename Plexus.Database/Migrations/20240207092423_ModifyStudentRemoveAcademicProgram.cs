using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentRemoveAcademicProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicProgramId",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicProgramId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
