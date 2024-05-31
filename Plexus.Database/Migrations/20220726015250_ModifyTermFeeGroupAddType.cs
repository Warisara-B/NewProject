using Microsoft.EntityFrameworkCore.Migrations;
using Plexus.Database.Enum.Payment;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyTermFeeGroupAddType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TermFeeGroups",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: TermFeePackageType.NORMAL.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TermFeeGroups");
        }
    }
}
