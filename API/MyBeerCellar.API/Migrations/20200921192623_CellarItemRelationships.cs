using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeerCellar.API.Migrations
{
    public partial class CellarItemRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeerContainerId",
                schema: "dbo",
                table: "CellarItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeerStyleId",
                schema: "dbo",
                table: "CellarItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CellarItem_BeerContainerId",
                schema: "dbo",
                table: "CellarItem",
                column: "BeerContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_CellarItem_BeerStyleId",
                schema: "dbo",
                table: "CellarItem",
                column: "BeerStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CellarItem_BeerContainer_BeerContainerId",
                schema: "dbo",
                table: "CellarItem",
                column: "BeerContainerId",
                principalSchema: "dbo",
                principalTable: "BeerContainer",
                principalColumn: "BeerContainerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CellarItem_BeerStyle_BeerStyleId",
                schema: "dbo",
                table: "CellarItem",
                column: "BeerStyleId",
                principalSchema: "dbo",
                principalTable: "BeerStyle",
                principalColumn: "StyleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CellarItem_BeerContainer_BeerContainerId",
                schema: "dbo",
                table: "CellarItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CellarItem_BeerStyle_BeerStyleId",
                schema: "dbo",
                table: "CellarItem");

            migrationBuilder.DropIndex(
                name: "IX_CellarItem_BeerContainerId",
                schema: "dbo",
                table: "CellarItem");

            migrationBuilder.DropIndex(
                name: "IX_CellarItem_BeerStyleId",
                schema: "dbo",
                table: "CellarItem");

            migrationBuilder.DropColumn(
                name: "BeerContainerId",
                schema: "dbo",
                table: "CellarItem");

            migrationBuilder.DropColumn(
                name: "BeerStyleId",
                schema: "dbo",
                table: "CellarItem");
        }
    }
}
