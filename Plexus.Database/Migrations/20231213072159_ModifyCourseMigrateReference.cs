using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCourseMigrateReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_GradeTemplates_GradeTemplateId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_TeachingTypes_TeachingTypeId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "MigrateReference",
                table: "CourseTopics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeachingTypeId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MigrateReference",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "GradeTemplateId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_GradeTemplates_GradeTemplateId",
                table: "Courses",
                column: "GradeTemplateId",
                principalTable: "GradeTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_TeachingTypes_TeachingTypeId",
                table: "Courses",
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

            migrationBuilder.AlterColumn<string>(
                name: "MigrateReference",
                table: "CourseTopics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeachingTypeId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MigrateReference",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GradeTemplateId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_GradeTemplates_GradeTemplateId",
                table: "Courses",
                column: "GradeTemplateId",
                principalTable: "GradeTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_TeachingTypes_TeachingTypeId",
                table: "Courses",
                column: "TeachingTypeId",
                principalTable: "TeachingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
