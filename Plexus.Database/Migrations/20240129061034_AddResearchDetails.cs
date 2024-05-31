using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddResearchDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompletedTermId",
                table: "ResearchProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ResearchProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ResearchProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StartTermId",
                table: "ResearchProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ResearchProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ResearchProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ResearchProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "ResearchProcesses",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAt",
                table: "ResearchProcesses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointedDate",
                table: "ResearchProcesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "ResearchProcesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AppointmentType",
                table: "ResearchProcesses",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefenseStatus",
                table: "ResearchProcesses",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ResearchProcesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "HasCommitteeConfirmed",
                table: "ResearchProcesses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationDay",
                table: "ResearchProcesses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "ResearchProcesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResearchCommittees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchCommittees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchCommittees_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchCommittees_ResearchProcesses_ResearchProcessId",
                        column: x => x.ResearchProcessId,
                        principalTable: "ResearchProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResearchProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchResources_ResearchProcesses_ResearchProcessId",
                        column: x => x.ResearchProcessId,
                        principalTable: "ResearchProcesses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResearchResources_ResearchProfiles_ResearchProfileId",
                        column: x => x.ResearchProfileId,
                        principalTable: "ResearchProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchProfiles_CompletedTermId",
                table: "ResearchProfiles",
                column: "CompletedTermId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchProfiles_StartTermId",
                table: "ResearchProfiles",
                column: "StartTermId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchCommittees_InstructorId",
                table: "ResearchCommittees",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchCommittees_ResearchProcessId",
                table: "ResearchCommittees",
                column: "ResearchProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_ResearchProcessId",
                table: "ResearchResources",
                column: "ResearchProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResources_ResearchProfileId",
                table: "ResearchResources",
                column: "ResearchProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchProfiles_Terms_CompletedTermId",
                table: "ResearchProfiles",
                column: "CompletedTermId",
                principalTable: "Terms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchProfiles_Terms_StartTermId",
                table: "ResearchProfiles",
                column: "StartTermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchProfiles_Terms_CompletedTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchProfiles_Terms_StartTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropTable(
                name: "ResearchCommittees");

            migrationBuilder.DropTable(
                name: "ResearchResources");

            migrationBuilder.DropIndex(
                name: "IX_ResearchProfiles_CompletedTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ResearchProfiles_StartTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "CompletedTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "StartTermId",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ResearchProfiles");

            migrationBuilder.DropColumn(
                name: "AppointedDate",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "DefenseStatus",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "HasCommitteeConfirmed",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "NotificationDay",
                table: "ResearchProcesses");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "ResearchProcesses");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "ResearchProcesses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAt",
                table: "ResearchProcesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
