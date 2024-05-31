using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddResearchTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchTemplates",
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
                    table.PrimaryKey("PK_ResearchTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchTemplates",
                schema: "localization",
                columns: table => new
                {
                    ResearchTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTemplates", x => new { x.ResearchTemplateId, x.Language });
                    table.ForeignKey(
                        name: "FK_ResearchTemplates_ResearchTemplates_ResearchTemplateId",
                        column: x => x.ResearchTemplateId,
                        principalTable: "ResearchTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchTemplateSequences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTemplateSequences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchTemplateSequences_ResearchTemplates_ResearchTemplateId",
                        column: x => x.ResearchTemplateId,
                        principalTable: "ResearchTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchTemplateSequences",
                schema: "localization",
                columns: table => new
                {
                    SequenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTemplateSequences", x => new { x.SequenceId, x.Language });
                    table.ForeignKey(
                        name: "FK_ResearchTemplateSequences_ResearchTemplateSequences_SequenceId",
                        column: x => x.SequenceId,
                        principalTable: "ResearchTemplateSequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchTemplateSequences_ResearchTemplateId",
                table: "ResearchTemplateSequences",
                column: "ResearchTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchTemplates",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "ResearchTemplateSequences",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "ResearchTemplateSequences");

            migrationBuilder.DropTable(
                name: "ResearchTemplates");
        }
    }
}
