using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class RenameFeeTypeToFeeItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseFees_FeeTypes_FeeTypeId",
                table: "CourseFees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipFeeItems_FeeTypes_FeeTypeId",
                table: "ScholarshipFeeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipFeeItemTransactions_FeeTypes_FeeTypeId",
                table: "ScholarshipFeeItemTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItems_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItems");

            migrationBuilder.DropTable(
                name: "FeeTypes",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "FeeTypes");

            migrationBuilder.RenameColumn(
                name: "FeeTypeId",
                table: "TermFeeGroupItems",
                newName: "FeeItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItems_FeeTypeId",
                table: "TermFeeGroupItems",
                newName: "IX_TermFeeGroupItems_FeeItemId");

            migrationBuilder.RenameColumn(
                name: "FeeTypeId",
                table: "ScholarshipFeeItemTransactions",
                newName: "FeeItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ScholarshipFeeItemTransactions_FeeTypeId",
                table: "ScholarshipFeeItemTransactions",
                newName: "IX_ScholarshipFeeItemTransactions_FeeItemId");

            migrationBuilder.RenameColumn(
                name: "FeeTypeId",
                table: "ScholarshipFeeItems",
                newName: "FeeItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ScholarshipFeeItems_FeeTypeId",
                table: "ScholarshipFeeItems",
                newName: "IX_ScholarshipFeeItems_FeeItemId");

            migrationBuilder.RenameColumn(
                name: "FeeTypeId",
                table: "CourseFees",
                newName: "FeeItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseFees_FeeTypeId",
                table: "CourseFees",
                newName: "IX_CourseFees_FeeItemId");

            migrationBuilder.CreateTable(
                name: "FeeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeItems_FeeGroups_FeeGroupId",
                        column: x => x.FeeGroupId,
                        principalTable: "FeeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeItems",
                schema: "localization",
                columns: table => new
                {
                    FeeItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeItems", x => new { x.FeeItemId, x.Language });
                    table.ForeignKey(
                        name: "FK_FeeItems_FeeItems_FeeItemId",
                        column: x => x.FeeItemId,
                        principalTable: "FeeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeItems_FeeGroupId",
                table: "FeeItems",
                column: "FeeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseFees_FeeItems_FeeItemId",
                table: "CourseFees",
                column: "FeeItemId",
                principalTable: "FeeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipFeeItems_FeeItems_FeeItemId",
                table: "ScholarshipFeeItems",
                column: "FeeItemId",
                principalTable: "FeeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipFeeItemTransactions_FeeItems_FeeItemId",
                table: "ScholarshipFeeItemTransactions",
                column: "FeeItemId",
                principalTable: "FeeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItems_FeeItems_FeeItemId",
                table: "TermFeeGroupItems",
                column: "FeeItemId",
                principalTable: "FeeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseFees_FeeItems_FeeItemId",
                table: "CourseFees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipFeeItems_FeeItems_FeeItemId",
                table: "ScholarshipFeeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipFeeItemTransactions_FeeItems_FeeItemId",
                table: "ScholarshipFeeItemTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItems_FeeItems_FeeItemId",
                table: "TermFeeGroupItems");

            migrationBuilder.DropTable(
                name: "FeeItems",
                schema: "localization");

            migrationBuilder.DropTable(
                name: "FeeItems");

            migrationBuilder.RenameColumn(
                name: "FeeItemId",
                table: "TermFeeGroupItems",
                newName: "FeeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItems_FeeItemId",
                table: "TermFeeGroupItems",
                newName: "IX_TermFeeGroupItems_FeeTypeId");

            migrationBuilder.RenameColumn(
                name: "FeeItemId",
                table: "ScholarshipFeeItemTransactions",
                newName: "FeeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ScholarshipFeeItemTransactions_FeeItemId",
                table: "ScholarshipFeeItemTransactions",
                newName: "IX_ScholarshipFeeItemTransactions_FeeTypeId");

            migrationBuilder.RenameColumn(
                name: "FeeItemId",
                table: "ScholarshipFeeItems",
                newName: "FeeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ScholarshipFeeItems_FeeItemId",
                table: "ScholarshipFeeItems",
                newName: "IX_ScholarshipFeeItems_FeeTypeId");

            migrationBuilder.RenameColumn(
                name: "FeeItemId",
                table: "CourseFees",
                newName: "FeeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseFees_FeeItemId",
                table: "CourseFees",
                newName: "IX_CourseFees_FeeTypeId");

            migrationBuilder.CreateTable(
                name: "FeeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeTypes_FeeGroups_FeeGroupId",
                        column: x => x.FeeGroupId,
                        principalTable: "FeeGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeeTypes",
                schema: "localization",
                columns: table => new
                {
                    FeeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTypes", x => new { x.FeeTypeId, x.Language });
                    table.ForeignKey(
                        name: "FK_FeeTypes_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeTypes_FeeGroupId",
                table: "FeeTypes",
                column: "FeeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseFees_FeeTypes_FeeTypeId",
                table: "CourseFees",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipFeeItems_FeeTypes_FeeTypeId",
                table: "ScholarshipFeeItems",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipFeeItemTransactions_FeeTypes_FeeTypeId",
                table: "ScholarshipFeeItemTransactions",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItems_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItems",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
