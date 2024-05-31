using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCurriculumInstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurriculumInstructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumInstructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurriculumInstructors_CurriculumVersions_CurriculumVersionId",
                        column: x => x.CurriculumVersionId,
                        principalTable: "CurriculumVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumInstructors_Employees_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumInstructors_InstructorRoles_InstructorRoleId",
                        column: x => x.InstructorRoleId,
                        principalTable: "InstructorRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumInstructors_CurriculumVersionId",
                table: "CurriculumInstructors",
                column: "CurriculumVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumInstructors_InstructorId",
                table: "CurriculumInstructors",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumInstructors_InstructorRoleId",
                table: "CurriculumInstructors",
                column: "InstructorRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumInstructors");
        }
    }
}
