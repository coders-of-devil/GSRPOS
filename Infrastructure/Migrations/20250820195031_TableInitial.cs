using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiningAreas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsReservable = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaWaitersEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    AreaId = table.Column<long>(type: "INTEGER", nullable: false),
                    WaiterId = table.Column<long>(type: "INTEGER", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaWaitersEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaWaitersEntries_DiningAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "DiningAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaWaitersEntries_Users_WaiterId",
                        column: x => x.WaiterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    AreaId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsOccupied = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsReservable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tables_DiningAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "DiningAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    TableId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsOccupied = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablePrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    TableId = table.Column<long>(type: "INTEGER", nullable: false),
                    DayType = table.Column<string>(type: "TEXT", nullable: false),
                    FromTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ToTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablePrices_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaWaitersEntries_AreaId",
                table: "AreaWaitersEntries",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaWaitersEntries_WaiterId",
                table: "AreaWaitersEntries",
                column: "WaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TableId",
                table: "Seats",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_TablePrices_TableId",
                table: "TablePrices",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_AreaId",
                table: "Tables",
                column: "AreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaWaitersEntries");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "TablePrices");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "DiningAreas");
        }
    }
}
