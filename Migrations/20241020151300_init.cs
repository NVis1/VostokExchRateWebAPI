using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VostokExchRateWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<short>(type: "INTEGER", maxLength: 10, nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Buy = table.Column<float>(type: "REAL", maxLength: 10, nullable: false),
                    Sell = table.Column<float>(type: "REAL", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
