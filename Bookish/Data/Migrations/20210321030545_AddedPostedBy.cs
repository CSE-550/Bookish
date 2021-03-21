using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookish.Data.Migrations
{
    public partial class AddedPostedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Posted_ById",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Posted_ById",
                table: "Posts",
                column: "Posted_ById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_Posted_ById",
                table: "Posts",
                column: "Posted_ById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_Posted_ById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Posted_ById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Posted_ById",
                table: "Posts");
        }
    }
}
