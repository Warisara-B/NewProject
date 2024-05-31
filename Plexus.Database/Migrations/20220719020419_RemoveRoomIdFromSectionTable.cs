using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RemoveRoomIdFromSectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Rooms_RoomId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RoomId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Sections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RoomId",
                table: "Sections",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Rooms_RoomId",
                table: "Sections",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
