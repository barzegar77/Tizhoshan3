using Microsoft.EntityFrameworkCore.Migrations;

namespace Tizhoshan.DataLayer.Migrations
{
    public partial class UpdateTblUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarName",
                table: "Users");
        }
    }
}
