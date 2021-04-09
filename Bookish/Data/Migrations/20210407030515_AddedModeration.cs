using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookish.Data.Migrations
{
    public partial class AddedModeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsModerator",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsModerator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Comments");
        }
    }
}
