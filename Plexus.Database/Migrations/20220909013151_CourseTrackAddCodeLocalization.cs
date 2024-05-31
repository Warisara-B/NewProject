using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class CourseTrackAddCodeLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CourseTracks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CourseTracks",
                schema: "localization",
                columns: table => new
                {
                    CourseTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTracks", x => new { x.CourseTrackId, x.Language });
                    table.ForeignKey(
                        name: "FK_CourseTracks_CourseTracks_CourseTrackId",
                        column: x => x.CourseTrackId,
                        principalTable: "CourseTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTracks_Code",
                table: "CourseTracks",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTracks",
                schema: "localization");

            migrationBuilder.DropIndex(
                name: "IX_CourseTracks_Code",
                table: "CourseTracks");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CourseTracks");
        }
    }
}
