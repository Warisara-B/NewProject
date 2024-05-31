using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifySectionStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Employees_MainInstructorId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Terms_TermId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsGhostSection",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "MinimumSeat",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "PlanningSeat",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "TotalWeeks",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Sections",
                newName: "SectionNo");

            migrationBuilder.RenameColumn(
                name: "MainInstructorId",
                table: "Sections",
                newName: "FacultyId");

            migrationBuilder.RenameColumn(
                name: "IsOutboundSection",
                table: "Sections",
                newName: "IsInvisibled");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_TermId_CourseId_Number",
                table: "Sections",
                newName: "IX_Sections_TermId_CourseId_SectionNo");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_MainInstructorId",
                table: "Sections",
                newName: "IX_Sections_FacultyId");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableSeat",
                table: "Sections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicLevelId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Batch",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CampusId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumVersionId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedAt",
                table: "Sections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatUsed",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Sections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentCodes",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sections",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "SectionExaminations",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "ExamType",
                table: "SectionExaminations",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "SectionExaminations",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "SectionExaminations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SectionExaminations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SectionClassPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionClassPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionClassPeriods_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SectionClassPeriods_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionInstructors",
                columns: table => new
                {
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionInstructors", x => new { x.InstructorId, x.SectionId });
                    table.ForeignKey(
                        name: "FK_SectionInstructors_Employees_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionInstructors_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionClassPeriodInstructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionClassPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionClassPeriodInstructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionClassPeriodInstructors_Employees_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionClassPeriodInstructors_SectionClassPeriods_SectionClassPeriodId",
                        column: x => x.SectionClassPeriodId,
                        principalTable: "SectionClassPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_AcademicLevelId",
                table: "Sections",
                column: "AcademicLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CampusId",
                table: "Sections",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CurriculumVersionId",
                table: "Sections",
                column: "CurriculumVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_DepartmentId",
                table: "Sections",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionClassPeriodInstructors_InstructorId",
                table: "SectionClassPeriodInstructors",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionClassPeriodInstructors_SectionClassPeriodId",
                table: "SectionClassPeriodInstructors",
                column: "SectionClassPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionClassPeriods_RoomId",
                table: "SectionClassPeriods",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionClassPeriods_SectionId",
                table: "SectionClassPeriods",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionInstructors_SectionId",
                table: "SectionInstructors",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_AcademicLevels_AcademicLevelId",
                table: "Sections",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Campuses_CampusId",
                table: "Sections",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_CurriculumVersions_CurriculumVersionId",
                table: "Sections",
                column: "CurriculumVersionId",
                principalTable: "CurriculumVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Departments_DepartmentId",
                table: "Sections",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Faculties_FacultyId",
                table: "Sections",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Terms_TermId",
                table: "Sections",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_AcademicLevels_AcademicLevelId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Campuses_CampusId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_CurriculumVersions_CurriculumVersionId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Departments_DepartmentId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Faculties_FacultyId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Terms_TermId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "SectionClassPeriodInstructors");

            migrationBuilder.DropTable(
                name: "SectionInstructors");

            migrationBuilder.DropTable(
                name: "SectionClassPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Sections_AcademicLevelId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CampusId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CurriculumVersionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_DepartmentId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "AcademicLevelId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Batch",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CurriculumVersionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "SeatUsed",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StudentCodes",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SectionExaminations");

            migrationBuilder.RenameColumn(
                name: "SectionNo",
                table: "Sections",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "IsInvisibled",
                table: "Sections",
                newName: "IsOutboundSection");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Sections",
                newName: "MainInstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_TermId_CourseId_SectionNo",
                table: "Sections",
                newName: "IX_Sections_TermId_CourseId_Number");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_FacultyId",
                table: "Sections",
                newName: "IX_Sections_MainInstructorId");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableSeat",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGhostSection",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MinimumSeat",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanningSeat",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedDate",
                table: "Sections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalWeeks",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "SectionExaminations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExamType",
                table: "SectionExaminations",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "SectionExaminations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "SectionExaminations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Employees_MainInstructorId",
                table: "Sections",
                column: "MainInstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Terms_TermId",
                table: "Sections",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
