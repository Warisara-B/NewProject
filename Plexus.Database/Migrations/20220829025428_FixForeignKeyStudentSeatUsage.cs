using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class FixForeignKeyStudentSeatUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeatUsages_Students_SectionId",
                table: "SectionSeatUsages");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSeatUsages_StudentId",
                table: "SectionSeatUsages",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeatUsages_Students_StudentId",
                table: "SectionSeatUsages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionSeatUsages_Students_StudentId",
                table: "SectionSeatUsages");

            migrationBuilder.DropIndex(
                name: "IX_SectionSeatUsages_StudentId",
                table: "SectionSeatUsages");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionSeatUsages_Students_SectionId",
                table: "SectionSeatUsages",
                column: "SectionId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
