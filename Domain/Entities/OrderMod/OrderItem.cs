using Domain.Entities.Common;
using Domain.Entities.MenuMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderMod
{
    public class OrderItem : BaseEntity
    {
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public long? ItemTypeId { get; set; }
        public MenuItemType? ItemType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; } = 1m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } = 0m;
        public bool IsComboItem { get; set; } = false;
        public long? ComboItemId { get; set; }
        public ComboItem? ComboItem { get; set; }
        [MaxLength(1000)]
        public string KitchenNotes { get; set; } = string.Empty;
        public bool IsKOTSent { get; set; } = false;
        public bool IsVoided { get; set; } = false;
        [Column(TypeName = "date")]
        public DateOnly? OrderDate { get; set; }
        public bool IsFOC { get; set; } = false;
        public string FOCReason { get; set; } = string.Empty;
        public ItemStatusEnum ItemStatus { get; set; } = ItemStatusEnum.Pending;
    }
}
