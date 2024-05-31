using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCourseFeeAddRateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CourseFees");

            migrationBuilder.AddColumn<Guid>(
                name: "RateTypeId",
                table: "CourseFees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CourseFees_RateTypeId",
                table: "CourseFees",
                column: "RateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseFees_RateTypes_RateTypeId",
                table: "CourseFees",
                column: "RateTypeId",
                principalTable: "RateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseFees_RateTypes_RateTypeId",
                table: "CourseFees");

            migrationBuilder.DropIndex(
                name: "IX_CourseFees_RateTypeId",
                table: "CourseFees");

            migrationBuilder.DropColumn(
                name: "RateTypeId",
                table: "CourseFees");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "CourseFees",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
