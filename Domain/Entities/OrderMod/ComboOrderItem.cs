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
    public class ComboOrderItem : BaseEntity
    {
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public long ComboItemId { get; set; }
        public ComboItem? ComboItem { get; set; }
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; } = 1m;
        public string Notes { get; set; } = string.Empty;
        public bool IsNoNeed { get; set; } = false;
        [Column(TypeName = "date")]
        public DateOnly? OrderDate { get; set; }
    }
}
