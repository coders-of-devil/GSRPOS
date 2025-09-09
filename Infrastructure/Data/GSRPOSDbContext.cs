using Domain.Entities.Common;
using Domain.Entities.CustomerMod;
using Domain.Entities.MenuMod;
using Domain.Entities.OrderMod;
using Domain.Entities.TableMod;
using Domain.Entities.UserMod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPass> UsersPass { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PreparationArea> PreparationAreas { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemNutrition> MenuItemNutritions { get; set; }
        public DbSet<ComboItem> ComboItems { get; set; }
        public DbSet<ComboItemDetail> ComboItemDetails { get; set; }
        public DbSet<DeliveryPlatform> DeliveryPlatforms { get; set; }
        public DbSet<MenuDeliveryPrice> MenuDeliveryPrices { get; set; }
        public DbSet<ModifierGroup> ModifierGroups { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }
        public DbSet<MenuItemModifierGroup> MenuItemModifierGroups { get; set; }
        public DbSet<PricelistTypes> PricelistTypes { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<MenuItemPriceListEntry> MenuItemPriceListEntries { get; set; }
        public DbSet<MenuItemType> MenuItemTypes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<DiningArea> DiningAreas { get; set; }
        public DbSet<DiningTable> Tables { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<AreaWaitersEntry> AreaWaitersEntries { get; set; }
        public DbSet<TablePrice> TablePrices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CreditHistory> CreditHistory { get; set; }
        public DbSet<RewardEarning> RewardEarnings { get; set; }
        public DbSet<RewardTransaction> RewardTransactions { get; set; }
        public DbSet<RewardUsage> RewardUsages { get; set; }
        public DbSet<KitchenDevice> KitchenDevices { get; set; }
        public DbSet<KitchenOrderRouting> KitchenOrderRoutings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ComboOrderItem> ComboOrderItems { get; set; }
        public DbSet<OrderItemModifier> OrderItemModifiers { get; set; }
        public DbSet<OrderPrepStatus> OrderPrepStatus { get; set; }
        public DbSet<DeliveryOrderTrack> DeliveryOrderTrack { get; set; }
        public DbSet<KOTLog> kOTLogs { get; set; }
        public DbSet<VoidOrderItemLog> VoidOrderItemLogs { get; set; }
        public DbSet<DiscountsLog> DiscountsLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
