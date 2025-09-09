using Domain.Entities.Common;
using Domain.Entities.CustomerMod;
using Domain.Entities.TableMod;
using Domain.Entities.UserMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderMod
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public OrderTypeEnum OrderType { get; set; } = OrderTypeEnum.TakeAway;
        public long? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public long? TableId { get; set; }
        public DiningTable? Table { get; set; }
        public long? SeatId { get; set; }
        public Seat? Seat { get; set; }
        public long? WaiterId { get; set; }
        public User? Waiter { get; set; }
        public OrderSourceEnum OrderSource { get; set; } = OrderSourceEnum.POS;
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Draft;
        public string DiscountCode { get; set; } = string.Empty;
        public long? CouponId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceCharge { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal PackingCharge { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryCharge { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal RoundOfAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossTotal { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetTotal { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal DueAmount { get; set; } = 0m;
        public bool IsBillPrinted { get; set; } = false;
        public KOTStatusEnum KOTStatus { get; set; } = KOTStatusEnum.None;
        public bool IsVoided { get; set; } = false;
        [Column(TypeName = "date")]
        public DateOnly? OrderDate { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Unpaid;
        public PaymentTypeEnum PaymentType { get; set; } = PaymentTypeEnum.None;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceivedAmount { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeGiven { get; set; } = 0m;
        public string DeliveryPerson { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public int NoOfItems { get; set; } = 0;
        public bool IsInvoiced { get; set; } = false;

    }
}
