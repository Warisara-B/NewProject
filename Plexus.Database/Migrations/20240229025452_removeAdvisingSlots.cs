using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class removeAdvisingSlots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvisingSlots");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvisingSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    StudentTermStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentTermTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvisingSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvisingSlots_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvisingSlots_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvisingSlots_StudentTerms_StudentTermStudentId_StudentTermTermId",
                        columns: x => new { x.StudentTermStudentId, x.StudentTermTermId },
                        principalTable: "StudentTerms",
                        principalColumns: new[] { "StudentId", "TermId" });
                    table.ForeignKey(
                        name: "FK_AdvisingSlots_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvisingSlots_InstructorId",
                table: "AdvisingSlots",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvisingSlots_StudentId",
                table: "AdvisingSlots",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvisingSlots_StudentTermStudentId_StudentTermTermId",
                table: "AdvisingSlots",
                columns: new[] { "StudentTermStudentId", "StudentTermTermId" });

            migrationBuilder.CreateIndex(
                name: "IX_AdvisingSlots_TermId",
                table: "AdvisingSlots",
                column: "TermId");
        }
    }
}
