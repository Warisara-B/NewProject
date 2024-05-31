using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyTuitionRateRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TuitionRateIndexs");

            migrationBuilder.DropTable(
                name: "TuitionRateIndexTransactions");

            migrationBuilder.DropTable(
                name: "TuitionRates");

            migrationBuilder.CreateTable(
                name: "CourseRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseRateIndexes",
                columns: table => new
                {
                    CourseRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRateIndexes", x => new { x.CourseRateId, x.RateTypeId, x.Index });
                    table.ForeignKey(
                        name: "FK_CourseRateIndexes_CourseRates_CourseRateId",
                        column: x => x.CourseRateId,
                        principalTable: "CourseRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRateIndexes_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseRateIndexTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRateIndexTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseRateIndexTransactions_CourseRates_CourseRateId",
                        column: x => x.CourseRateId,
                        principalTable: "CourseRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRateIndexTransactions_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseRateIndexes_RateTypeId",
                table: "CourseRateIndexes",
                column: "RateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRateIndexTransactions_CourseRateId",
                table: "CourseRateIndexTransactions",
                column: "CourseRateId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRateIndexTransactions_RateTypeId",
                table: "CourseRateIndexTransactions",
                column: "RateTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseRateIndexes");

            migrationBuilder.DropTable(
                name: "CourseRateIndexTransactions");

            migrationBuilder.DropTable(
                name: "CourseRates");

            migrationBuilder.CreateTable(
                name: "TuitionRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuitionRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TuitionRateIndexs",
                columns: table => new
                {
                    TuitionRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuitionRateIndexs", x => new { x.TuitionRateId, x.RateTypeId, x.Index });
                    table.ForeignKey(
                        name: "FK_TuitionRateIndexs_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TuitionRateIndexs_TuitionRates_TuitionRateId",
                        column: x => x.TuitionRateId,
                        principalTable: "TuitionRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TuitionRateIndexTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuitionRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuitionRateIndexTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TuitionRateIndexTransactions_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TuitionRateIndexTransactions_TuitionRates_TuitionRateId",
                        column: x => x.TuitionRateId,
                        principalTable: "TuitionRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TuitionRateIndexs_RateTypeId",
                table: "TuitionRateIndexs",
                column: "RateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TuitionRateIndexTransactions_RateTypeId",
                table: "TuitionRateIndexTransactions",
                column: "RateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TuitionRateIndexTransactions_TuitionRateId",
                table: "TuitionRateIndexTransactions",
                column: "TuitionRateId");
        }
    }
}
