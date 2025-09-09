using Domain.Entities.Common;
using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderMod
{
    public class VoidOrderItemLog : BaseEntity
    {
        public bool IsWholeOrder { get; set; } = false;
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public bool IsOnlyItem { get; set; } = false;
        public long? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public long VoidedBy { get; set; }
        public User? VoidedByUser { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime? VoidedAt { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
