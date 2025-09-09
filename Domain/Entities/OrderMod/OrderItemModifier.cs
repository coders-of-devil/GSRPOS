using Domain.Entities.Common;
using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderMod
{
    public class OrderItemModifier : BaseEntity
    {
        [Required]
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public long OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public long ModifierId { get; set; }
        public Modifier? Modifier { get; set; }
        public string Note { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0m;
    }
}
