using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RenameTableTermFeeGroupAndItemAndLocalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermFeeGroupItems");

            migrationBuilder.DropTable(
                name: "TermFeeGroups",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "TermFeeGroups");

            migrationBuilder.CreateTable(
                name: "TermFeePackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeePackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermFeeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermFeePackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermType = table.Column<int>(type: "int", nullable: true),
                    RecurringType = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermFeeItems_FeeItems_FeeItemId",
                        column: x => x.FeeItemId,
                        principalTable: "FeeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TermFeeItems_TermFeePackages_TermFeePackageId",
                        column: x => x.TermFeePackageId,
                        principalTable: "TermFeePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TermFeePackages",
                schema: "localization",
                columns: table => new
                {
                    TermFeePackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeePackages", x => new { x.TermFeePackageId, x.Language });
                    table.ForeignKey(
                        name: "FK_TermFeePackages_TermFeePackages_TermFeePackageId",
                        column: x => x.TermFeePackageId,
                        principalTable: "TermFeePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeItems_FeeItemId",
                table: "TermFeeItems",
                column: "FeeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeItems_TermFeePackageId",
                table: "TermFeeItems",
                column: "TermFeePackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermFeeItems");

            migrationBuilder.DropTable(
                name: "TermFeePackages",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "TermFeePackages");

            migrationBuilder.CreateTable(
                name: "TermFeeGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermFeeGroupItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermFeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RecurringType = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermType = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroupItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermFeeGroupItems_FeeItems_FeeItemId",
                        column: x => x.FeeItemId,
                        principalTable: "FeeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TermFeeGroupItems_TermFeeGroups_TermFeeGroupId",
                        column: x => x.TermFeeGroupId,
                        principalTable: "TermFeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TermFeeGroups",
                schema: "localization",
                columns: table => new
                {
                    TermFeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermFeeGroups", x => new { x.TermFeeGroupId, x.Language });
                    table.ForeignKey(
                        name: "FK_TermFeeGroups_TermFeeGroups_TermFeeGroupId",
                        column: x => x.TermFeeGroupId,
                        principalTable: "TermFeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeGroupItems_FeeItemId",
                table: "TermFeeGroupItems",
                column: "FeeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TermFeeGroupItems_TermFeeGroupId",
                table: "TermFeeGroupItems",
                column: "TermFeeGroupId");
        }
    }
}
