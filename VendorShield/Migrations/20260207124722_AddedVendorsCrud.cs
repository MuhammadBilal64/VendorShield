using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorShield.Migrations
{
    /// <inheritdoc />
    public partial class AddedVendorsCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vendors");
        }
    }
}
