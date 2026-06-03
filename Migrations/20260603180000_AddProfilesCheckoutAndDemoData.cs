using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetroGameStore.Migrations
{
    public partial class AddProfilesCheckoutAndDemoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FulfillmentType",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "Store Pickup");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "Cash");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "admin@rgsdemo.com", "Nora Admin", "Admin@123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "staff@rgsdemo.com", "Omar Inventory", "Staff@123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "lina@rgsdemo.com", "Lina Carter", "Demo@123" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "Password", "Role" },
                values: new object[,]
                {
                    { 4, "maya@rgsdemo.com", "Maya Collins", "Demo@123", 2 },
                    { 5, "adam@rgsdemo.com", "Adam Brooks", "Demo@123", 2 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "DeliveryAddress", "FulfillmentType", "GameId", "OrderDate", "PaymentMethod", "Quantity", "Status", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 101, null, "Store Pickup", 1, new DateTime(2026, 6, 1, 10, 30, 0), "Cash", 1, 0, 29.99m, 3 },
                    { 102, "Demo Street 12", "Delivery to Home", 3, new DateTime(2026, 6, 1, 12, 10, 0), "Cash", 2, 2, 69.98m, 3 },
                    { 103, null, "Store Pickup", 7, new DateTime(2026, 6, 2, 15, 45, 0), "Cash", 1, 0, 54.99m, 4 },
                    { 104, "Demo Avenue 7", "Delivery to Home", 8, new DateTime(2026, 6, 3, 9, 20, 0), "Cash", 1, 0, 19.99m, 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 101);
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 102);
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 103);
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 104);

            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "admin@retrogames.com", "Admin User", "admin123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "staff@retrogames.com", "Staff Member", "staff123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FullName", "Password" },
                values: new object[] { "customer@retrogames.com", "John Customer", "customer123" });

            migrationBuilder.DropColumn(name: "DeliveryAddress", table: "Orders");
            migrationBuilder.DropColumn(name: "FulfillmentType", table: "Orders");
            migrationBuilder.DropColumn(name: "PaymentMethod", table: "Orders");
        }
    }
}
