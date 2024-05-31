using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddResearchProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchProcesses_ResearchTemplates_ResearchTemplateId",
                        column: x => x.ResearchTemplateId,
                        principalTable: "ResearchTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchProfiles_ResearchTemplates_ResearchTemplateId",
                        column: x => x.ResearchTemplateId,
                        principalTable: "ResearchTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchProcesses",
                schema: "localization",
                columns: table => new
                {
                    ProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchProcesses", x => new { x.ProcessId, x.Language });
                    table.ForeignKey(
                        name: "FK_ResearchProcesses_ResearchProcesses_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "ResearchProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchProcesses_ResearchTemplateId",
                table: "ResearchProcesses",
                column: "ResearchTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchProfiles_ResearchTemplateId",
                table: "ResearchProfiles",
                column: "ResearchTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchProcesses",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "ResearchProfiles");

            migrationBuilder.DropTable(
                name: "ResearchProcesses");
        }
    }
}
