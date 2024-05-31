using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddTeachingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "ShortName",
                schema: "localization",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "OtherCredit",
                table: "Courses",
                newName: "OtherHour");

            migrationBuilder.RenameColumn(
                name: "LectureCredit",
                table: "Courses",
                newName: "LectureHour");

            migrationBuilder.RenameColumn(
                name: "LabCredit",
                table: "Courses",
                newName: "LabHour");

            migrationBuilder.AddColumn<Guid>(
                name: "TeachingTypeId",
                table: "SectionDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GradeTemplateId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Hour",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "TeachingTypeId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "GradeTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeachingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeTemplateDetails",
                columns: table => new
                {
                    GradeTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTemplateDetails", x => new { x.GradeTemplateId, x.GradeId });
                    table.ForeignKey(
                        name: "FK_GradeTemplateDetails_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeTemplateDetails_GradeTemplates_GradeTemplateId",
                        column: x => x.GradeTemplateId,
                        principalTable: "GradeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachingTypes",
                schema: "localization",
                columns: table => new
                {
                    TeachingTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingTypes", x => new { x.TeachingTypeId, x.Language });
                    table.ForeignKey(
                        name: "FK_TeachingTypes_TeachingTypes_TeachingTypeId",
                        column: x => x.TeachingTypeId,
                        principalTable: "TeachingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionDetails_TeachingTypeId",
                table: "SectionDetails",
                column: "TeachingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GradeTemplateId",
                table: "Courses",
                column: "GradeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeachingTypeId",
                table: "Courses",
                column: "TeachingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTemplateDetails_GradeId",
                table: "GradeTemplateDetails",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTemplates_Name",
                table: "GradeTemplates",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_GradeTemplates_GradeTemplateId",
                table: "Courses",
                column: "GradeTemplateId",
                principalTable: "GradeTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_TeachingTypes_TeachingTypeId",
                table: "Courses",
                column: "TeachingTypeId",
                principalTable: "TeachingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionDetails_TeachingTypes_TeachingTypeId",
                table: "SectionDetails",
                column: "TeachingTypeId",
                principalTable: "TeachingTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_GradeTemplates_GradeTemplateId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_TeachingTypes_TeachingTypeId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionDetails_TeachingTypes_TeachingTypeId",
                table: "SectionDetails");

            migrationBuilder.DropTable(
                name: "GradeTemplateDetails");

            migrationBuilder.DropTable(
                name: "TeachingTypes",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "GradeTemplates");

            migrationBuilder.DropTable(
                name: "TeachingTypes");

            migrationBuilder.DropIndex(
                name: "IX_SectionDetails_TeachingTypeId",
                table: "SectionDetails");

            migrationBuilder.DropIndex(
                name: "IX_Courses_GradeTemplateId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeachingTypeId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeachingTypeId",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "GradeTemplateId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeachingTypeId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "OtherHour",
                table: "Courses",
                newName: "OtherCredit");

            migrationBuilder.RenameColumn(
                name: "LectureHour",
                table: "Courses",
                newName: "LectureCredit");

            migrationBuilder.RenameColumn(
                name: "LabHour",
                table: "Courses",
                newName: "LabCredit");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SectionDetails",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                schema: "localization",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
