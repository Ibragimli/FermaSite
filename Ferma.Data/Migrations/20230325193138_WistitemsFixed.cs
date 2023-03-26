using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class WistitemsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishItems_AspNetUsers_AppUserId1",
                table: "WishItems");

            migrationBuilder.DropIndex(
                name: "IX_WishItems_AppUserId1",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "WishItems");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "WishItems",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_WishItems_AppUserId",
                table: "WishItems",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishItems_AspNetUsers_AppUserId",
                table: "WishItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishItems_AspNetUsers_AppUserId",
                table: "WishItems");

            migrationBuilder.DropIndex(
                name: "IX_WishItems_AppUserId",
                table: "WishItems");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "WishItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "WishItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishItems_AppUserId1",
                table: "WishItems",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WishItems_AspNetUsers_AppUserId1",
                table: "WishItems",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
