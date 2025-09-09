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
    public class DiscountsLog : BaseEntity
    {
        public bool IsOrderDiscount { get; set; } = false;
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public bool IsItemDiscount { get; set; } = false;
        public long? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;
        public bool IsCouponBased { get; set; } = false;
        public string DiscountCoupon { get; set; } = string.Empty;
        public string CouponSource { get; set; } = string.Empty;
        public long? AppliedBy { get; set; }
        public User? AppliedByUser { get; set; }
        public DateTime? AppliedAt { get; set; }
    }
}
