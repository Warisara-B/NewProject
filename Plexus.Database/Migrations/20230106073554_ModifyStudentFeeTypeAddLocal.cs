using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentFeeTypeAddLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentFeeTypes_Code",
                table: "StudentFeeTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "StudentFeeTypes");

            migrationBuilder.CreateTable(
                name: "StudentFeeTypes",
                schema: "localization",
                columns: table => new
                {
                    StudentFeeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeTypes", x => new { x.StudentFeeTypeId, x.Language });
                    table.ForeignKey(
                        name: "FK_StudentFeeTypes_StudentFeeTypes_StudentFeeTypeId",
                        column: x => x.StudentFeeTypeId,
                        principalTable: "StudentFeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFeeTypes",
                schema: "localization");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "StudentFeeTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeTypes_Code",
                table: "StudentFeeTypes",
                column: "Code",
                unique: true);
        }
    }
}
