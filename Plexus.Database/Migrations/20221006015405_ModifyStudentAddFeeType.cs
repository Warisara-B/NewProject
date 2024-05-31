using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentAddFeeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentFeeTypeId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentFeeTypeId",
                table: "Students",
                column: "StudentFeeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentFeeTypes_StudentFeeTypeId",
                table: "Students",
                column: "StudentFeeTypeId",
                principalTable: "StudentFeeTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentFeeTypes_StudentFeeTypeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentFeeTypeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentFeeTypeId",
                table: "Students");
        }
    }
}
