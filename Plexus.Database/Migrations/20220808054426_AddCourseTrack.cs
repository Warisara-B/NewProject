using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddCourseTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseTrackDetails",
                columns: table => new
                {
                    CourseTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTrackDetails", x => new { x.CourseTrackId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseTrackDetails_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseTrackDetails_CourseTracks_CourseTrackId",
                        column: x => x.CourseTrackId,
                        principalTable: "CourseTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseTracks",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseTracks", x => new { x.StudentId, x.CourseTrackId });
                    table.ForeignKey(
                        name: "FK_StudentCourseTracks_CourseTracks_CourseTrackId",
                        column: x => x.CourseTrackId,
                        principalTable: "CourseTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourseTracks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTrackDetails_CourseId",
                table: "CourseTrackDetails",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseTracks_CourseTrackId",
                table: "StudentCourseTracks",
                column: "CourseTrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTrackDetails");

            migrationBuilder.DropTable(
                name: "StudentCourseTracks");

            migrationBuilder.DropTable(
                name: "CourseTracks");
        }
    }
}
