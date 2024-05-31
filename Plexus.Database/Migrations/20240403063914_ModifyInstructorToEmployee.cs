using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyInstructorToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendarInstructors_Instructors_InstructorId",
                table: "AcademicCalendarInstructors");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvisingSlots_Instructors_InstructorId",
                table: "AdvisingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Instructors_InstructorId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkNews_Instructors_InstructorId",
                table: "BookmarkNews");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendations_Instructors_InstructorId",
                table: "CourseRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTopicInstructors_Instructors_InstructorId",
                table: "CourseTopicInstructors");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorAcademicLevels_Instructors_InstructorId",
                table: "InstructorAcademicLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorAddresses_Instructors_InstructorId",
                table: "InstructorAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchCommittees_Instructors_InstructorId",
                table: "ResearchCommittees");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionDetails_Instructors_InstructorId",
                table: "SectionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Instructors_MainInstructorId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTerms_Instructors_AdvisorId",
                table: "StudentTerms");

            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "InstructorWorkStatuses");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "AcademicCalendarInstructors",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicCalendarInstructors_InstructorId",
                table: "AcademicCalendarInstructors",
                newName: "IX_AcademicCalendarInstructors_EmployeeId");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CareerPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniversityEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternativeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AcademicPositions_AcademicPositionId",
                        column: x => x.AcademicPositionId,
                        principalTable: "AcademicPositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_CareerPositions_CareerPositionId",
                        column: x => x.CareerPositionId,
                        principalTable: "CareerPositions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducationalBackgrounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Institute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEducationalBackgrounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEducationalBackgrounds_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "localization",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => new { x.EmployeeId, x.Language });
                    table.ForeignKey(
                        name: "FK_Employees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    OfficeRoom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkInformations_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeWorkInformations_EmployeeGroups_EmployeeGroupId",
                        column: x => x.EmployeeGroupId,
                        principalTable: "EmployeeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeWorkInformations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkInformations_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeExpertises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Minor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeExpertises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeExpertises_EmployeeWorkInformations_WorkInformationId",
                        column: x => x.WorkInformationId,
                        principalTable: "EmployeeWorkInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducationalBackgrounds_EmployeeId",
                table: "EmployeeEducationalBackgrounds",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExpertises_WorkInformationId",
                table: "EmployeeExpertises",
                column: "WorkInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AcademicPositionId",
                table: "Employees",
                column: "AcademicPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CareerPositionId",
                table: "Employees",
                column: "CareerPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkInformations_DepartmentId",
                table: "EmployeeWorkInformations",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkInformations_EmployeeGroupId",
                table: "EmployeeWorkInformations",
                column: "EmployeeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkInformations_EmployeeId",
                table: "EmployeeWorkInformations",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkInformations_FacultyId",
                table: "EmployeeWorkInformations",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendarInstructors_Employees_EmployeeId",
                table: "AcademicCalendarInstructors",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisingSlots_Employees_InstructorId",
                table: "AdvisingSlots",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Employees_InstructorId",
                table: "ApplicationUsers",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkNews_Employees_InstructorId",
                table: "BookmarkNews",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendations_Employees_InstructorId",
                table: "CourseRecommendations",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTopicInstructors_Employees_InstructorId",
                table: "CourseTopicInstructors",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorAcademicLevels_Employees_InstructorId",
                table: "InstructorAcademicLevels",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorAddresses_Employees_InstructorId",
                table: "InstructorAddresses",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchCommittees_Employees_InstructorId",
                table: "ResearchCommittees",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionDetails_Employees_InstructorId",
                table: "SectionDetails",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Employees_MainInstructorId",
                table: "Sections",
                column: "MainInstructorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTerms_Employees_AdvisorId",
                table: "StudentTerms",
                column: "AdvisorId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendarInstructors_Employees_EmployeeId",
                table: "AcademicCalendarInstructors");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvisingSlots_Employees_InstructorId",
                table: "AdvisingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Employees_InstructorId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkNews_Employees_InstructorId",
                table: "BookmarkNews");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendations_Employees_InstructorId",
                table: "CourseRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTopicInstructors_Employees_InstructorId",
                table: "CourseTopicInstructors");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorAcademicLevels_Employees_InstructorId",
                table: "InstructorAcademicLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorAddresses_Employees_InstructorId",
                table: "InstructorAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchCommittees_Employees_InstructorId",
                table: "ResearchCommittees");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionDetails_Employees_InstructorId",
                table: "SectionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Employees_MainInstructorId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTerms_Employees_AdvisorId",
                table: "StudentTerms");

            migrationBuilder.DropTable(
                name: "EmployeeEducationalBackgrounds");

            migrationBuilder.DropTable(
                name: "EmployeeExpertises");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "EmployeeWorkInformations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "AcademicCalendarInstructors",
                newName: "InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicCalendarInstructors_EmployeeId",
                table: "AcademicCalendarInstructors",
                newName: "IX_AcademicCalendarInstructors_InstructorId");

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IndentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "localization",
                columns: table => new
                {
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => new { x.InstructorId, x.Language });
                    table.ForeignKey(
                        name: "FK_Instructors_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorWorkStatuses",
                columns: table => new
                {
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeRoom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWorkStatuses", x => x.InstructorId);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_EmployeeGroups_EmployeeGroupId",
                        column: x => x.EmployeeGroupId,
                        principalTable: "EmployeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWorkStatuses_InstructorTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "InstructorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_FacultyId",
                table: "Instructors",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_DepartmentId",
                table: "InstructorWorkStatuses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_EmployeeGroupId",
                table: "InstructorWorkStatuses",
                column: "EmployeeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_FacultyId",
                table: "InstructorWorkStatuses",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWorkStatuses_TypeId",
                table: "InstructorWorkStatuses",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendarInstructors_Instructors_InstructorId",
                table: "AcademicCalendarInstructors",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisingSlots_Instructors_InstructorId",
                table: "AdvisingSlots",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Instructors_InstructorId",
                table: "ApplicationUsers",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkNews_Instructors_InstructorId",
                table: "BookmarkNews",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendations_Instructors_InstructorId",
                table: "CourseRecommendations",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTopicInstructors_Instructors_InstructorId",
                table: "CourseTopicInstructors",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorAcademicLevels_Instructors_InstructorId",
                table: "InstructorAcademicLevels",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorAddresses_Instructors_InstructorId",
                table: "InstructorAddresses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchCommittees_Instructors_InstructorId",
                table: "ResearchCommittees",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionDetails_Instructors_InstructorId",
                table: "SectionDetails",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Instructors_MainInstructorId",
                table: "Sections",
                column: "MainInstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTerms_Instructors_AdvisorId",
                table: "StudentTerms",
                column: "AdvisorId",
                principalTable: "Instructors",
                principalColumn: "Id");
        }
    }
}
