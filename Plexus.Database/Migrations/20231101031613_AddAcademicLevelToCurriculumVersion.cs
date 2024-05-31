using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddAcademicLevelToCurriculumVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FormalName",
                schema: "localization",
                table: "CurriculumVersions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "RequiredGPA",
                table: "CurriculumVersions",
                newName: "TotalYear");

            migrationBuilder.RenameColumn(
                name: "RequiredCredit",
                table: "CurriculumVersions",
                newName: "TotalCredit");

            migrationBuilder.RenameColumn(
                name: "FormalName",
                table: "CurriculumVersions",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "DegreeName",
                schema: "localization",
                table: "CurriculumVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicLevelId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicProgramId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "CurriculumVersions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DegreeName",
                table: "CurriculumVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "EndBatchCode",
                table: "CurriculumVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedGraduatingCredit",
                table: "CurriculumVersions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "StartBatchCode",
                table: "CurriculumVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVersions_AcademicLevelId",
                table: "CurriculumVersions",
                column: "AcademicLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVersions_AcademicProgramId",
                table: "CurriculumVersions",
                column: "AcademicProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVersions_DepartmentId",
                table: "CurriculumVersions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVersions_FacultyId",
                table: "CurriculumVersions",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_AcademicLevels_AcademicLevelId",
                table: "CurriculumVersions",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_AcademicPrograms_AcademicProgramId",
                table: "CurriculumVersions",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_Departments_DepartmentId",
                table: "CurriculumVersions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_Faculties_FacultyId",
                table: "CurriculumVersions",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_AcademicLevels_AcademicLevelId",
                table: "CurriculumVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_AcademicPrograms_AcademicProgramId",
                table: "CurriculumVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_Departments_DepartmentId",
                table: "CurriculumVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_Faculties_FacultyId",
                table: "CurriculumVersions");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumVersions_AcademicLevelId",
                table: "CurriculumVersions");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumVersions_AcademicProgramId",
                table: "CurriculumVersions");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumVersions_DepartmentId",
                table: "CurriculumVersions");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumVersions_FacultyId",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "DegreeName",
                schema: "localization",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "AcademicLevelId",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "AcademicProgramId",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "DegreeName",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "EndBatchCode",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "ExpectedGraduatingCredit",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "CurriculumVersions");

            migrationBuilder.DropColumn(
                name: "StartBatchCode",
                table: "CurriculumVersions");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "localization",
                table: "CurriculumVersions",
                newName: "FormalName");

            migrationBuilder.RenameColumn(
                name: "TotalYear",
                table: "CurriculumVersions",
                newName: "RequiredGPA");

            migrationBuilder.RenameColumn(
                name: "TotalCredit",
                table: "CurriculumVersions",
                newName: "RequiredCredit");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CurriculumVersions",
                newName: "FormalName");
        }
    }
}
