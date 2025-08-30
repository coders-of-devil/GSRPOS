using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPermissionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUntil",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "PermissionGroups",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsOverWriting = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsUnderWriting = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTemporary = table.Column<bool>(type: "INTEGER", nullable: false),
                    AssignedUntil = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PermissionGroups",
                newName: "name");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedUntil",
                table: "UserRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "UserRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
