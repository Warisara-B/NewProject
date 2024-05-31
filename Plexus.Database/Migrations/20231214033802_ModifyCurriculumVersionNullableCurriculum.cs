using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCurriculumVersionNullableCurriculum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_Curriculums_CurriculumId",
                table: "CurriculumVersions");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurriculumId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_Curriculums_CurriculumId",
                table: "CurriculumVersions",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumVersions_Curriculums_CurriculumId",
                table: "CurriculumVersions");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurriculumId",
                table: "CurriculumVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumVersions_Curriculums_CurriculumId",
                table: "CurriculumVersions",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
