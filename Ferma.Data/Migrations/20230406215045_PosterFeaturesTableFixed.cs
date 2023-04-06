using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class PosterFeaturesTableFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationEndDate",
                table: "PosterFeatures");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateActive",
                table: "PosterFeatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDateActive",
                table: "PosterFeatures");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationEndDate",
                table: "PosterFeatures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
