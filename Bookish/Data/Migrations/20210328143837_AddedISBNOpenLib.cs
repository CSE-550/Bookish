using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookish.Data.Migrations
{
    public partial class AddedISBNOpenLib : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Posts",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorksId",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BookTitle",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "WorksId",
                table: "Posts");
        }
    }
}
