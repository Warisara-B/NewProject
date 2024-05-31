using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCurriculumVersionSpecialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurriculumVersionSpecializations",
                columns: table => new
                {
                    CurriculumVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicSpecializationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumVersionSpecializations", x => new { x.CurriculumVersionId, x.AcademicSpecializationId });
                    table.ForeignKey(
                        name: "FK_CurriculumVersionSpecializations_AcademicSpecializations_AcademicSpecializationId",
                        column: x => x.AcademicSpecializationId,
                        principalTable: "AcademicSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumVersionSpecializations_CurriculumVersions_CurriculumVersionId",
                        column: x => x.CurriculumVersionId,
                        principalTable: "CurriculumVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVersionSpecializations_AcademicSpecializationId",
                table: "CurriculumVersionSpecializations",
                column: "AcademicSpecializationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumVersionSpecializations");
        }
    }
}
