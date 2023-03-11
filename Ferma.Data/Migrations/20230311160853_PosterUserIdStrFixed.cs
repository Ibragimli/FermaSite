using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class PosterUserIdStrFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosterUserIds_AspNetUsers_AppUserId1",
                table: "PosterUserIds");

            migrationBuilder.DropIndex(
                name: "IX_PosterUserIds_AppUserId1",
                table: "PosterUserIds");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PosterUserIds");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PosterUserIds",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PosterUserIds_AppUserId",
                table: "PosterUserIds",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PosterUserIds_AspNetUsers_AppUserId",
                table: "PosterUserIds",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosterUserIds_AspNetUsers_AppUserId",
                table: "PosterUserIds");

            migrationBuilder.DropIndex(
                name: "IX_PosterUserIds_AppUserId",
                table: "PosterUserIds");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "PosterUserIds",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PosterUserIds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosterUserIds_AppUserId1",
                table: "PosterUserIds",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PosterUserIds_AspNetUsers_AppUserId1",
                table: "PosterUserIds",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
