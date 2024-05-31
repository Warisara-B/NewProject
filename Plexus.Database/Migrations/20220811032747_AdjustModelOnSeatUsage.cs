using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AdjustModelOnSeatUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeatUsages_SectionSeats_MasterSeatId",
                table: "SectionSeatUsages");

            migrationBuilder.RenameColumn(
                name: "MasterSeatId",
                table: "SectionSeatUsages",
                newName: "ReferenceSeatId");

            migrationBuilder.RenameIndex(
                name: "IX_SectionSeatUsages_MasterSeatId",
                table: "SectionSeatUsages",
                newName: "IX_SectionSeatUsages_ReferenceSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeatUsages_SectionSeats_ReferenceSeatId",
                table: "SectionSeatUsages",
                column: "ReferenceSeatId",
                principalTable: "SectionSeats",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeatUsages_SectionSeats_ReferenceSeatId",
                table: "SectionSeatUsages");

            migrationBuilder.RenameColumn(
                name: "ReferenceSeatId",
                table: "SectionSeatUsages",
                newName: "MasterSeatId");

            migrationBuilder.RenameIndex(
                name: "IX_SectionSeatUsages_ReferenceSeatId",
                table: "SectionSeatUsages",
                newName: "IX_SectionSeatUsages_MasterSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeatUsages_SectionSeats_MasterSeatId",
                table: "SectionSeatUsages",
                column: "MasterSeatId",
                principalTable: "SectionSeats",
                principalColumn: "Id");
        }
    }
}
