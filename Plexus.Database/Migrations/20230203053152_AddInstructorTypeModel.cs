using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddInstructorTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Instructors");

            migrationBuilder.CreateTable(
                name: "InstructorTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorTypes",
                schema: "localization",
                columns: table => new
                {
                    InstructorTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorTypes", x => new { x.InstructorTypeId, x.Language });
                    table.ForeignKey(
                        name: "FK_InstructorTypes_InstructorTypes_InstructorTypeId",
                        column: x => x.InstructorTypeId,
                        principalTable: "InstructorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorTypes",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "InstructorTypes");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Instructors",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }
    }
}
