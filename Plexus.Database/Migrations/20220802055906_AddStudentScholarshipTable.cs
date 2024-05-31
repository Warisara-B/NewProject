using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddStudentScholarshipTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentScholarships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScholarShipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedLimitBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartTerm = table.Column<int>(type: "int", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScholarships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScholarships_Scholarships_ScholarShipId",
                        column: x => x.ScholarShipId,
                        principalTable: "Scholarships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentScholarships_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentScholarshipReserveBudgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentScholarshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScholarshipReserveBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScholarshipReserveBudgets_StudentScholarships_StudentScholarshipId",
                        column: x => x.StudentScholarshipId,
                        principalTable: "StudentScholarships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentScholarshipUsages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentScholarshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReserveBudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScholarshipUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScholarshipUsages_StudentScholarshipReserveBudgets_ReserveBudgetId",
                        column: x => x.ReserveBudgetId,
                        principalTable: "StudentScholarshipReserveBudgets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentScholarshipUsages_StudentScholarships_StudentScholarshipId",
                        column: x => x.StudentScholarshipId,
                        principalTable: "StudentScholarships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarshipReserveBudgets_StudentScholarshipId",
                table: "StudentScholarshipReserveBudgets",
                column: "StudentScholarshipId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarships_ScholarShipId",
                table: "StudentScholarships",
                column: "ScholarShipId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarships_StudentId",
                table: "StudentScholarships",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarshipUsages_ReserveBudgetId",
                table: "StudentScholarshipUsages",
                column: "ReserveBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarshipUsages_StudentScholarshipId",
                table: "StudentScholarshipUsages",
                column: "StudentScholarshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentScholarshipUsages");

            migrationBuilder.DropTable(
                name: "StudentScholarshipReserveBudgets");

            migrationBuilder.DropTable(
                name: "StudentScholarships");
        }
    }
}
