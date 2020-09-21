using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeerCellar.API.Migrations
{
    public partial class BeerContainerAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeerContainer",
                schema: "dbo",
                columns: table => new
                {
                    BeerContainerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerType = table.Column<string>(maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerContainer", x => x.BeerContainerId);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerContainer",
                columns: new[] { "BeerContainerId", "ContainerType" },
                values: new object[] { 1, "Can" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerContainer",
                columns: new[] { "BeerContainerId", "ContainerType" },
                values: new object[] { 2, "Bottle" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BeerContainer",
                columns: new[] { "BeerContainerId", "ContainerType" },
                values: new object[] { 3, "Growler" });

            migrationBuilder.CreateIndex(
                name: "IX_BeerContainer_ContainerType",
                schema: "dbo",
                table: "BeerContainer",
                column: "ContainerType",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeerContainer",
                schema: "dbo");
        }
    }
}
