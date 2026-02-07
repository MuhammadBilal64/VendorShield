using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorShield.Migrations
{
    /// <inheritdoc />
    public partial class Addingcolumsforpurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHighQuality",
                table: "PurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnTime",
                table: "PurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHighQuality",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "IsOnTime",
                table: "PurchaseOrders");
        }
    }
}
