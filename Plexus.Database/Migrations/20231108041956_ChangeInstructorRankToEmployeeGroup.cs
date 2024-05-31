using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ChangeInstructorRankToEmployeeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorWorkStatuses_InstructorRanks_RankId",
                table: "InstructorWorkStatuses");

            migrationBuilder.DropTable(
                name: "InstructorRanks",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "InstructorRanks");

            migrationBuilder.RenameColumn(
                name: "RankId",
                table: "InstructorWorkStatuses",
                newName: "EmployeeGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorWorkStatuses_RankId",
                table: "InstructorWorkStatuses",
                newName: "IX_InstructorWorkStatuses_EmployeeGroupId");

            migrationBuilder.AddColumn<string>(
                name: "IndentificationNumber",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeGroups",
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
                    table.PrimaryKey("PK_EmployeeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeGroups",
                schema: "localization",
                columns: table => new
                {
                    EmployeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeGroups", x => new { x.EmployeeGroupId, x.Language });
                    table.ForeignKey(
                        name: "FK_EmployeeGroups_EmployeeGroups_EmployeeGroupId",
                        column: x => x.EmployeeGroupId,
                        principalTable: "EmployeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorWorkStatuses_EmployeeGroups_EmployeeGroupId",
                table: "InstructorWorkStatuses",
                column: "EmployeeGroupId",
                principalTable: "EmployeeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorWorkStatuses_EmployeeGroups_EmployeeGroupId",
                table: "InstructorWorkStatuses");

            migrationBuilder.DropTable(
                name: "EmployeeGroups",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "EmployeeGroups");

            migrationBuilder.DropColumn(
                name: "IndentificationNumber",
                table: "Instructors");

            migrationBuilder.RenameColumn(
                name: "EmployeeGroupId",
                table: "InstructorWorkStatuses",
                newName: "RankId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorWorkStatuses_EmployeeGroupId",
                table: "InstructorWorkStatuses",
                newName: "IX_InstructorWorkStatuses_RankId");

            migrationBuilder.CreateTable(
                name: "InstructorRanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorRanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorRanks",
                schema: "localization",
                columns: table => new
                {
                    InstructorRankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorRanks", x => new { x.InstructorRankId, x.Language });
                    table.ForeignKey(
                        name: "FK_InstructorRanks_InstructorRanks_InstructorRankId",
                        column: x => x.InstructorRankId,
                        principalTable: "InstructorRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorWorkStatuses_InstructorRanks_RankId",
                table: "InstructorWorkStatuses",
                column: "RankId",
                principalTable: "InstructorRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
