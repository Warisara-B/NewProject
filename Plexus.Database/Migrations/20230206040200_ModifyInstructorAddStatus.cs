using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyInstructorAddStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Faculties_FacultyId",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_FacultyId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "PersonalEmailAddress",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Instructors");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Instructors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InstructorWorkStatuses",
                columns: table => new
                {
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficeRoom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWorkStatuses", x => x.InstructorId);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_InstructorRanks_RankId",
                        column: x => x.RankId,
                        principalTable: "InstructorRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_InstructorTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "InstructorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_DepartmentId",
                table: "InstructorWorkStatuses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_FacultyId",
                table: "InstructorWorkStatuses",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_RankId",
                table: "InstructorWorkStatuses",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_TypeId",
                table: "InstructorWorkStatuses",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorWorkStatuses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Instructors");

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "Instructors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalEmailAddress",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Instructors",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_FacultyId",
                table: "Instructors",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Faculties_FacultyId",
                table: "Instructors",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }
    }
}
