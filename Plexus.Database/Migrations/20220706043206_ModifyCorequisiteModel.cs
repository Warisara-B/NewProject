using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCorequisiteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Corequisites",
                table: "Corequisites");

            migrationBuilder.DropIndex(
                name: "IX_Corequisites_CurriculumVersionId",
                table: "Corequisites");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Corequisites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Corequisites",
                table: "Corequisites",
                columns: new[] { "CurriculumVersionId", "CourseId", "CorequisiteCourseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Corequisites",
                table: "Corequisites");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Corequisites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Corequisites",
                table: "Corequisites",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Corequisites_CurriculumVersionId",
                table: "Corequisites",
                column: "CurriculumVersionId");
        }
    }
}
