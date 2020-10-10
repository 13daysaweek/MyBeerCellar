using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeerCellar.API.Migrations
{
    public partial class CellarItemAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CellarItem",
                schema: "dbo",
                columns: table => new
                {
                    CellarItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(maxLength: 255, nullable: false),
                    YearProduced = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellarItem", x => x.CellarItemId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CellarItem_ItemName_YearProduced",
                schema: "dbo",
                table: "CellarItem",
                columns: new[] { "ItemName", "YearProduced" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CellarItem",
                schema: "dbo");
        }
    }
}
