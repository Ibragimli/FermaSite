using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class UserAutehnticationCountTableAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "UserAuthentications",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "UserAuthentications",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "UserAuthentications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "UserAuthentications");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "UserAuthentications",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "UserAuthentications",
                newName: "code");
        }
    }
}
