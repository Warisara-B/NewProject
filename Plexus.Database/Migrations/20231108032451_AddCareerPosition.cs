using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCareerPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_AcademicPositions_AcademicPositionId",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_AcademicPositionId",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "AcademicPositionId",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.CreateTable(
                name: "CareerPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CareerPositions",
                schema: "localization",
                columns: table => new
                {
                    CareerPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPositions", x => new { x.CareerPositionId, x.Language });
                    table.ForeignKey(
                        name: "FK_CareerPositions_CareerPositions_CareerPositionId",
                        column: x => x.CareerPositionId,
                        principalTable: "CareerPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareerPositions",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "CareerPositions");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicPositionId",
                schema: "localization",
                table: "Faculties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_AcademicPositionId",
                schema: "localization",
                table: "Faculties",
                column: "AcademicPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_AcademicPositions_AcademicPositionId",
                schema: "localization",
                table: "Faculties",
                column: "AcademicPositionId",
                principalTable: "AcademicPositions",
                principalColumn: "Id");
        }
    }
}
