using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentAddAcademicProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AcademicLevels_AcademicLevelId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "BatchCode",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicLevelId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicProgramId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudyPlanId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudyPlanId",
                table: "Students",
                column: "StudyPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AcademicLevels_AcademicLevelId",
                table: "Students",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AcademicLevels_AcademicLevelId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudyPlanId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicProgramId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudyPlanId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "BatchCode",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicLevelId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AcademicLevels_AcademicLevelId",
                table: "Students",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
