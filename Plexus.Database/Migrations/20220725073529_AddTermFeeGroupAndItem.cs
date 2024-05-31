using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddTermFeeGroupAndItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TermFeeGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermFeeGroupItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermFeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermType = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    RecurringType = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroupItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermFeeGroupItem_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TermFeeGroupItem_TermFeeGroups_TermFeeGroupId",
                        column: x => x.TermFeeGroupId,
                        principalTable: "TermFeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeGroupItem_FeeTypeId",
                table: "TermFeeGroupItem",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeGroupItem_TermFeeGroupId",
                table: "TermFeeGroupItem",
                column: "TermFeeGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermFeeGroupItem");

            migrationBuilder.DropTable(
                name: "TermFeeGroups");
        }
    }
}
