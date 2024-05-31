using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class UpdateTuitionRateIndexTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TuitionRateIndexs",
                table: "TuitionRateIndexs");

            migrationBuilder.AddColumn<Guid>(
                name: "RateTypeId",
                table: "TuitionRateIndexTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RateTypeId",
                table: "TuitionRateIndexs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TuitionRateIndexs",
                table: "TuitionRateIndexs",
                columns: new[] { "TuitionRateId", "RateTypeId", "Index" });

            migrationBuilder.CreateIndex(
                name: "IX_TuitionRateIndexTransactions_RateTypeId",
                table: "TuitionRateIndexTransactions",
                column: "RateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TuitionRateIndexs_RateTypeId",
                table: "TuitionRateIndexs",
                column: "RateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TuitionRateIndexs_RateTypes_RateTypeId",
                table: "TuitionRateIndexs",
                column: "RateTypeId",
                principalTable: "RateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TuitionRateIndexTransactions_RateTypes_RateTypeId",
                table: "TuitionRateIndexTransactions",
                column: "RateTypeId",
                principalTable: "RateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TuitionRateIndexs_RateTypes_RateTypeId",
                table: "TuitionRateIndexs");

            migrationBuilder.DropForeignKey(
                name: "FK_TuitionRateIndexTransactions_RateTypes_RateTypeId",
                table: "TuitionRateIndexTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TuitionRateIndexTransactions_RateTypeId",
                table: "TuitionRateIndexTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TuitionRateIndexs",
                table: "TuitionRateIndexs");

            migrationBuilder.DropIndex(
                name: "IX_TuitionRateIndexs_RateTypeId",
                table: "TuitionRateIndexs");

            migrationBuilder.DropColumn(
                name: "RateTypeId",
                table: "TuitionRateIndexTransactions");

            migrationBuilder.DropColumn(
                name: "RateTypeId",
                table: "TuitionRateIndexs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TuitionRateIndexs",
                table: "TuitionRateIndexs",
                columns: new[] { "TuitionRateId", "Index" });
        }
    }
}
