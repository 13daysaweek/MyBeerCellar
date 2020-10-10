using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeerCellar.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BeerStyle",
                schema: "dbo",
                columns: table => new
                {
                    StyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StyleName = table.Column<string>(maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyle", x => x.StyleId);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerStyle",
                columns: new[] { "StyleId", "StyleName" },
                values: new object[] { 1, "American IPA" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerStyle",
                columns: new[] { "StyleId", "StyleName" },
                values: new object[] { 2, "New England IPA" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerStyle",
                columns: new[] { "StyleId", "StyleName" },
                values: new object[] { 3, "Imperial Stout" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeerStyle",
                schema: "dbo");
        }
    }
}
