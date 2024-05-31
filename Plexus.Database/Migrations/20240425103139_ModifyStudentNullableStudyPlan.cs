using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentNullableStudyPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudyPlanId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudyPlanId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyPlans_StudyPlanId",
                table: "Students",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
