using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferma.Data.Migrations
{
    public partial class SubCategoryPosterFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosterFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Describe = table.Column<string>(maxLength: 3000, nullable: false),
                    Email = table.Column<string>(maxLength: 70, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 15, nullable: false),
                    Price = table.Column<int>(nullable: false),
                    PriceCurrency = table.Column<bool>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    WishCount = table.Column<int>(nullable: false),
                    IsVip = table.Column<bool>(nullable: false),
                    IsPremium = table.Column<bool>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    PosterStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosterFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosterFeatures_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PosterFeatures_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosterFeatures_CityId",
                table: "PosterFeatures",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PosterFeatures_SubCategoryId",
                table: "PosterFeatures",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosterFeatures");

            migrationBuilder.DropTable(
                name: "SubCategories");
        }
    }
}
