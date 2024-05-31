using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddSectionSeatAndSeatUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectionSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalSeat = table.Column<int>(type: "int", nullable: false),
                    SeatUsed = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionSeats_SectionSeats_MasterSeatId",
                        column: x => x.MasterSeatId,
                        principalTable: "SectionSeats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SectionSeatUsages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    MasterSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionSeatUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionSeatUsages_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SectionSeatUsages_SectionSeats_MasterSeatId",
                        column: x => x.MasterSeatId,
                        principalTable: "SectionSeats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SectionSeatUsages_SectionSeats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "SectionSeats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SectionSeatUsages_Students_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeats_MasterSeatId",
                table: "SectionSeats",
                column: "MasterSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeatUsages_MasterSeatId",
                table: "SectionSeatUsages",
                column: "MasterSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeatUsages_SeatId",
                table: "SectionSeatUsages",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeatUsages_SectionId",
                table: "SectionSeatUsages",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionSeatUsages");

            migrationBuilder.DropTable(
                name: "SectionSeats");
        }
    }
}
