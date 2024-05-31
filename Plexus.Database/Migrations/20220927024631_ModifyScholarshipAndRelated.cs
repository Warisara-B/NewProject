using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyScholarshipAndRelated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "StudentScholarshipUsages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalRemark",
                table: "StudentScholarships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "StudentScholarships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "StudentScholarships",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "StudentScholarships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxGPA",
                table: "Scholarships",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Scholarships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBudget",
                table: "Scholarships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "StudentScholarshipUsages");

            migrationBuilder.DropColumn(
                name: "ApprovalRemark",
                table: "StudentScholarships");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "StudentScholarships");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "StudentScholarships");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "StudentScholarships");

            migrationBuilder.DropColumn(
                name: "MaxGPA",
                table: "Scholarships");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Scholarships");

            migrationBuilder.DropColumn(
                name: "TotalBudget",
                table: "Scholarships");
        }
    }
}
