using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeerCellar.API.Migrations
{
    public partial class StyleUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BeerStyle_StyleName",
                schema: "dbo",
                table: "BeerStyle",
                column: "StyleName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BeerStyle_StyleName",
                schema: "dbo",
                table: "BeerStyle");
        }
    }
}
