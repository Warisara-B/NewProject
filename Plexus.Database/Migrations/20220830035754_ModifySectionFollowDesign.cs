using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Plexus.Database.Enum.Academic.Section;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifySectionFollowDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Sections",
                newName: "IsClosed");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawable",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGhostSection",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutboundSection",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MainInstructorId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumSeat",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanningSeat",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Sections",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: SectionStatus.PENDING.ToString());

            migrationBuilder.AddColumn<Guid>(
                name: "InstructorId",
                table: "SectionDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SectionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SectionDetails",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "None");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_MainInstructorId",
                table: "Sections",
                column: "MainInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDetails_InstructorId",
                table: "SectionDetails",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionDetails_Instructors_InstructorId",
                table: "SectionDetails",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Instructors_MainInstructorId",
                table: "Sections",
                column: "MainInstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionDetails_Instructors_InstructorId",
                table: "SectionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Instructors_MainInstructorId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_MainInstructorId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_SectionDetails_InstructorId",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "IsWithdrawable",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsGhostSection",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsOutboundSection",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "MainInstructorId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "MinimumSeat",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "PlanningSeat",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SectionDetails");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Sections",
                newName: "IsActive");
        }
    }
}
