using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyStudentStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LivingStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "StudentGuardians");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "IsMainAddress",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StudentAddresses");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Students",
                newName: "UniversityEmail");

            migrationBuilder.RenameColumn(
                name: "PersonalEmailAddress",
                table: "Students",
                newName: "StudentStatusRemark");

            migrationBuilder.RenameColumn(
                name: "CardExpiryDate",
                table: "Students",
                newName: "BankAccountUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "BirthState",
                table: "Students",
                newName: "ProfileImageUrl");

            migrationBuilder.RenameColumn(
                name: "BirthProvince",
                table: "Students",
                newName: "PreviousCode");

            migrationBuilder.RenameColumn(
                name: "BirthCity",
                table: "Students",
                newName: "PhoneNumber2");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "StudentAddresses",
                newName: "Soi");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                schema: "localization",
                table: "Students",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                table: "Students",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicProgramId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AlternativeEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNo",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBranch",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedCredit",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber1",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationCredit",
                table: "Students",
                type: "int",
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

            migrationBuilder.AddColumn<int>(
                name: "TransferredCredit",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitizenNo",
                table: "StudentGuardians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moo",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Road",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "CourseTracks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "AcademicSpecializations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students",
                column: "StudentId");

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deformations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passports_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGuardianLocalizations",
                schema: "localization",
                columns: table => new
                {
                    StudentGuardianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGuardianLocalizations", x => new { x.StudentGuardianId, x.Language });
                    table.ForeignKey(
                        name: "FK_StudentGuardianLocalizations_StudentGuardians_StudentGuardianId",
                        column: x => x.StudentGuardianId,
                        principalTable: "StudentGuardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentBankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBankAccounts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTracks_StudentId",
                table: "CourseTracks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSpecializations_StudentId",
                table: "AcademicSpecializations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Deformations_StudentId",
                table: "Deformations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_StudentId",
                table: "Passports",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBankAccounts_BankId",
                table: "StudentBankAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBankAccounts_StudentId",
                table: "StudentBankAccounts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicSpecializations_Students_StudentId",
                table: "AcademicSpecializations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTracks_Students_StudentId",
                table: "CourseTracks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicSpecializations_Students_StudentId",
                table: "AcademicSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTracks_Students_StudentId",
                table: "CourseTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AcademicPrograms_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Deformations");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.DropTable(
                name: "StudentBankAccounts");

            migrationBuilder.DropTable(
                name: "StudentGuardianLocalizations",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicProgramId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_CourseTracks_StudentId",
                table: "CourseTracks");

            migrationBuilder.DropIndex(
                name: "IX_AcademicSpecializations_StudentId",
                table: "AcademicSpecializations");

            migrationBuilder.DropColumn(
                name: "AcademicProgramId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AlternativeEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BankAccountNo",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BankBranch",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CompletedCredit",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Line",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PersonalEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RegistrationCredit",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentStatusEffectiveDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TransferredCredit",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CitizenNo",
                table: "StudentGuardians");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "Moo",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "Road",
                table: "StudentAddresses");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourseTracks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AcademicSpecializations");

            migrationBuilder.RenameColumn(
                name: "UniversityEmail",
                table: "Students",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "StudentStatusRemark",
                table: "Students",
                newName: "PersonalEmailAddress");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "Students",
                newName: "BirthState");

            migrationBuilder.RenameColumn(
                name: "PreviousCode",
                table: "Students",
                newName: "BirthProvince");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber2",
                table: "Students",
                newName: "BirthCity");

            migrationBuilder.RenameColumn(
                name: "BankAccountUpdatedAt",
                table: "Students",
                newName: "CardExpiryDate");

            migrationBuilder.RenameColumn(
                name: "Soi",
                table: "StudentAddresses",
                newName: "City");

            migrationBuilder.AlterColumn<int>(
                name: "Language",
                schema: "localization",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                table: "Students",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicStatus",
                table: "Students",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDate",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionType",
                table: "Students",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LivingStatus",
                table: "Students",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Students",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "StudentGuardians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainAddress",
                table: "StudentAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StudentAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                schema: "localization",
                table: "Students",
                columns: new[] { "StudentId", "Language" });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
