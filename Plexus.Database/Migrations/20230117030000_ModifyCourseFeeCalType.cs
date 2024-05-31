using Microsoft.EntityFrameworkCore.Migrations;
using Plexus.Database.Enum.Payment;

#nullable disable

namespace Plexus.Database.Migrations
{
    public partial class ModifyCourseFeeCalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalculationType",
                table: "CourseFees",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: CalculationType.FIXED.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculationType",
                table: "CourseFees");
        }
    }
}
