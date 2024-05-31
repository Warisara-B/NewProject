using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddStudentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentFeeTypes_StudentFeeTypeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentFeeTypeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentFeeTypeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentStatusEffectiveDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentStatusRemark",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentAcademicStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAcademicStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAcademicStatus_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicStatus_StudentId",
                table: "StudentAcademicStatus",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAcademicStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentFeeTypeId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentStatus",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StudentStatusEffectiveDate",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentStatusRemark",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentFeeTypeId",
                table: "Students",
                column: "StudentFeeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentFeeTypes_StudentFeeTypeId",
                table: "Students",
                column: "StudentFeeTypeId",
                principalTable: "StudentFeeTypes",
                principalColumn: "Id");
        }
    }
}
