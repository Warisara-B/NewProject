using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudyCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegistrationChannel",
                table: "StudyCourses",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "RegistrationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationChannel = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationLogs_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationLogCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudyCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationLogCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationLogCourses_RegistrationLogs_RegistrationLogId",
                        column: x => x.RegistrationLogId,
                        principalTable: "RegistrationLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationLogCourses_StudyCourses_StudyCourseId",
                        column: x => x.StudyCourseId,
                        principalTable: "StudyCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationLogCourses_RegistrationLogId",
                table: "RegistrationLogCourses",
                column: "RegistrationLogId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationLogCourses_StudyCourseId",
                table: "RegistrationLogCourses",
                column: "StudyCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationLogs_StudentId",
                table: "RegistrationLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationLogs_TermId",
                table: "RegistrationLogs",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationLogCourses");

            migrationBuilder.DropTable(
                name: "RegistrationLogs");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationChannel",
                table: "StudyCourses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
