using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class ExpiratonDateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PosterFeatures");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "UserAuthentications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateDisabled",
                table: "PosterFeatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDatePremium",
                table: "PosterFeatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateVip",
                table: "PosterFeatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationEndDate",
                table: "PosterFeatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "UserAuthentications");

            migrationBuilder.DropColumn(
                name: "ExpirationDateDisabled",
                table: "PosterFeatures");

            migrationBuilder.DropColumn(
                name: "ExpirationDatePremium",
                table: "PosterFeatures");

            migrationBuilder.DropColumn(
                name: "ExpirationDateVip",
                table: "PosterFeatures");

            migrationBuilder.DropColumn(
                name: "ExpirationEndDate",
                table: "PosterFeatures");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PosterFeatures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
