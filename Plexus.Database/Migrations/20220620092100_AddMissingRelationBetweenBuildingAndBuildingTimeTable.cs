using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddMissingRelationBetweenBuildingAndBuildingTimeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_BuildingAvailableTimes_Buildings_BuildingId",
                table: "BuildingAvailableTimes",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingAvailableTimes_Buildings_BuildingId",
                table: "BuildingAvailableTimes");
        }
    }
}
