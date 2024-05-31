using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class UpdateTermFeeItemAndTuitionRateConditionNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItem_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItem_TermFeeGroups_TermFeeGroupId",
                table: "TermFeeGroupItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TermFeeGroupItem",
                table: "TermFeeGroupItem");

            migrationBuilder.RenameTable(
                name: "TermFeeGroupItem",
                newName: "TermFeeGroupItems");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItem_TermFeeGroupId",
                table: "TermFeeGroupItems",
                newName: "IX_TermFeeGroupItems_TermFeeGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItem_FeeTypeId",
                table: "TermFeeGroupItems",
                newName: "IX_TermFeeGroupItems_FeeTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "TuitionRates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "TermFeeGroupItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TermFeeGroupItems",
                table: "TermFeeGroupItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItems_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItems",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItems_TermFeeGroups_TermFeeGroupId",
                table: "TermFeeGroupItems",
                column: "TermFeeGroupId",
                principalTable: "TermFeeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItems_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TermFeeGroupItems_TermFeeGroups_TermFeeGroupId",
                table: "TermFeeGroupItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TermFeeGroupItems",
                table: "TermFeeGroupItems");

            migrationBuilder.RenameTable(
                name: "TermFeeGroupItems",
                newName: "TermFeeGroupItem");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItems_TermFeeGroupId",
                table: "TermFeeGroupItem",
                newName: "IX_TermFeeGroupItem_TermFeeGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_TermFeeGroupItems_FeeTypeId",
                table: "TermFeeGroupItem",
                newName: "IX_TermFeeGroupItem_FeeTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "TuitionRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "TermFeeGroupItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TermFeeGroupItem",
                table: "TermFeeGroupItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItem_FeeTypes_FeeTypeId",
                table: "TermFeeGroupItem",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TermFeeGroupItem_TermFeeGroups_TermFeeGroupId",
                table: "TermFeeGroupItem",
                column: "TermFeeGroupId",
                principalTable: "TermFeeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
