using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetroGameStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<int>(type: "INTEGER", nullable: false),
                    Genre = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CoverImageUrl", "Genre", "LowStockThreshold", "Platform", "Price", "ReleaseYear", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, "/images/games/crash_bandicoot.png", 6, 3, 0, 29.99m, 1996, 5, "Crash Bandicoot" },
                    { 2, "/images/games/gran_turismo_3.png", 4, 3, 1, 24.99m, 2001, 2, "Gran Turismo 3" },
                    { 3, "/images/games/pokemon_red.png", 2, 3, 2, 34.99m, 1996, 8, "Pokémon Red" },
                    { 4, "/images/games/zelda_minish_cap.png", 1, 3, 3, 39.99m, 2004, 3, "The Legend of Zelda: Minish Cap" },
                    { 5, "/images/games/super_mario_64.png", 6, 3, 4, 44.99m, 1996, 1, "Super Mario 64" },
                    { 6, "/images/games/zelda_ocarina.png", 1, 3, 4, 49.99m, 1998, 2, "The Legend of Zelda: Ocarina of Time" },
                    { 7, "/images/games/smash_bros_melee.png", 5, 3, 5, 54.99m, 2001, 4, "Super Smash Bros. Melee" },
                    { 8, "/images/games/resident_evil.png", 0, 3, 0, 19.99m, 1996, 6, "Resident Evil" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "admin@retrogames.com", "Admin User", "admin123", 0 },
                    { 2, "staff@retrogames.com", "Staff Member", "staff123", 1 },
                    { 3, "customer@retrogames.com", "John Customer", "customer123", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GameId",
                table: "Orders",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
