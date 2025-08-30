using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MenuSecondsChange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "MenuItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryPriceChange",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PricelistId",
                table: "MenuItemTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeliveryPriceChange",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PricelistId",
                table: "MenuItemTypes");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "MenuItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "MenuItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
