using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MenuInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "UsersPass",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncTime",
                table: "UsersPass",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DeliveryPlatforms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPlatforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsParentCategory = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentCategoryId = table.Column<long>(type: "INTEGER", nullable: true),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IconPath = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategories_MenuCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModifierGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxSelectable = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreparationAreas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparationAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricelistTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricelistTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComboItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboItems_MenuCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ModifierGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modifiers_ModifierGroups_ModifierGroupId",
                        column: x => x.ModifierGroupId,
                        principalTable: "ModifierGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    StockKeepingUnit = table.Column<string>(type: "TEXT", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpectedPreparationTime = table.Column<int>(type: "INTEGER", nullable: false),
                    PreparationAreaId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItems_PreparationAreas_PreparationAreaId",
                        column: x => x.PreparationAreaId,
                        principalTable: "PreparationAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    TypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTemporary = table.Column<bool>(type: "INTEGER", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_PricelistTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PricelistTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboItemDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ComboId = table.Column<long>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboItemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboItemDetails_ComboItems_ComboId",
                        column: x => x.ComboId,
                        principalTable: "ComboItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboItemDetails_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuDeliveryPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    PlatformId = table.Column<long>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuDeliveryPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuDeliveryPrices_DeliveryPlatforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "DeliveryPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuDeliveryPrices_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemModifierGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    ModifierGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemModifierGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemModifierGroups_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemModifierGroups_ModifierGroups_ModifierGroupId",
                        column: x => x.ModifierGroupId,
                        principalTable: "ModifierGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemNutritions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Calories = table.Column<string>(type: "TEXT", nullable: false),
                    Protien = table.Column<string>(type: "TEXT", nullable: false),
                    Fat = table.Column<string>(type: "TEXT", nullable: false),
                    Sugar = table.Column<string>(type: "TEXT", nullable: false),
                    Sodium = table.Column<string>(type: "TEXT", nullable: false),
                    CarboHydrates = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemNutritions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemNutritions_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemPriceListEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    PriceListId = table.Column<long>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemPriceListEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemPriceListEntries_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemPriceListEntries_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboItemDetails_ComboId",
                table: "ComboItemDetails",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItemDetails_MenuItemId",
                table: "ComboItemDetails",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItems_CategoryId",
                table: "ComboItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_ParentCategoryId",
                table: "MenuCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDeliveryPrices_MenuItemId",
                table: "MenuDeliveryPrices",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDeliveryPrices_PlatformId",
                table: "MenuDeliveryPrices",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemModifierGroups_MenuItemId",
                table: "MenuItemModifierGroups",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemModifierGroups_ModifierGroupId",
                table: "MenuItemModifierGroups",
                column: "ModifierGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemNutritions_MenuItemId",
                table: "MenuItemNutritions",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemPriceListEntries_MenuItemId",
                table: "MenuItemPriceListEntries",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemPriceListEntries_PriceListId",
                table: "MenuItemPriceListEntries",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_PreparationAreaId",
                table: "MenuItems",
                column: "PreparationAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_ModifierGroupId",
                table: "Modifiers",
                column: "ModifierGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_TypeId",
                table: "PriceLists",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboItemDetails");

            migrationBuilder.DropTable(
                name: "MenuDeliveryPrices");

            migrationBuilder.DropTable(
                name: "MenuItemModifierGroups");

            migrationBuilder.DropTable(
                name: "MenuItemNutritions");

            migrationBuilder.DropTable(
                name: "MenuItemPriceListEntries");

            migrationBuilder.DropTable(
                name: "Modifiers");

            migrationBuilder.DropTable(
                name: "ComboItems");

            migrationBuilder.DropTable(
                name: "DeliveryPlatforms");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "ModifierGroups");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "PreparationAreas");

            migrationBuilder.DropTable(
                name: "PricelistTypes");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "UsersPass");

            migrationBuilder.DropColumn(
                name: "SyncTime",
                table: "UsersPass");
        }
    }
}
