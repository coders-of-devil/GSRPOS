using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MenuSeconds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableToday",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MenuItemTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsTaxable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTaxExclusive = table.Column<bool>(type: "INTEGER", nullable: false),
                    TaxRate = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMultiplePriceForItem = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemTypes");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "IsAvailableToday",
                table: "MenuItems");
        }
    }
}
