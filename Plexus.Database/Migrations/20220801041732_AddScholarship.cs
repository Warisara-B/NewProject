using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddScholarship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scholarships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScholarshipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sponsor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LimitBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearDuration = table.Column<int>(type: "int", nullable: false),
                    MinGPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsRepeatCourseApplied = table.Column<bool>(type: "bit", nullable: false),
                    IsAllCoverage = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scholarships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scholarships_ScholarshipTypes_ScholarshipTypeId",
                        column: x => x.ScholarshipTypeId,
                        principalTable: "ScholarshipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scholarships_ScholarshipTypeId",
                table: "Scholarships",
                column: "ScholarshipTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scholarships");
        }
    }
}
