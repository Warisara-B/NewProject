using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class AddScholarshipFeeItemAndTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScholarshipFeeItems",
                columns: table => new
                {
                    ScholarshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipFeeItems", x => new { x.ScholarshipId, x.FeeTypeId });
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeItems_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeItems_Scholarships_ScholarshipId",
                        column: x => x.ScholarshipId,
                        principalTable: "Scholarships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipFeeItemTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScholarshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipFeeItemTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeItemTransactions_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeItemTransactions_Scholarships_ScholarshipId",
                        column: x => x.ScholarshipId,
                        principalTable: "Scholarships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeItems_FeeTypeId",
                table: "ScholarshipFeeItems",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeItemTransactions_FeeTypeId",
                table: "ScholarshipFeeItemTransactions",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeItemTransactions_ScholarshipId",
                table: "ScholarshipFeeItemTransactions",
                column: "ScholarshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScholarshipFeeItems");

            migrationBuilder.DropTable(
                name: "ScholarshipFeeItemTransactions");
        }
    }
}
