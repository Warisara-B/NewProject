using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddAcademicPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicPositionId",
                schema: "localization",
                table: "Faculties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicPositions",
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
                    table.PrimaryKey("PK_AcademicPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicPositions",
                schema: "localization",
                columns: table => new
                {
                    AcademicPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicPositions", x => new { x.AcademicPositionId, x.Language });
                    table.ForeignKey(
                        name: "FK_AcademicPositions_AcademicPositions_AcademicPositionId",
                        column: x => x.AcademicPositionId,
                        principalTable: "AcademicPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_AcademicPositions_AcademicPositionId",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.DropTable(
                name: "AcademicPositions",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "AcademicPositions");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_AcademicPositionId",
                schema: "localization",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "AcademicPositionId",
                schema: "localization",
                table: "Faculties");
        }
    }
}
