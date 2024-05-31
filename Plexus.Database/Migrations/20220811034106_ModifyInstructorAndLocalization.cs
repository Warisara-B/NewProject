using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyInstructorAndLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardImageUrl",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "Instructors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalEmailAddress",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Instructors",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Instructors",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "localization",
                columns: table => new
                {
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => new { x.InstructorId, x.Language });
                    table.ForeignKey(
                        name: "FK_Instructors_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_FacultyId",
                table: "Instructors",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Faculties_FacultyId",
                table: "Instructors",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Faculties_FacultyId",
                table: "Instructors");

            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "localization");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_FacultyId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "CardImageUrl",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "PersonalEmailAddress",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Instructors");
        }
    }
}
