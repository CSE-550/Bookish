using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookish.Data.Migrations
{
    public partial class AddedCommentedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Commented_ById",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Commented_ById",
                table: "Comments",
                column: "Commented_ById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_Commented_ById",
                table: "Comments",
                column: "Commented_ById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_Commented_ById",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Commented_ById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Commented_ById",
                table: "Comments");
        }
    }
}
