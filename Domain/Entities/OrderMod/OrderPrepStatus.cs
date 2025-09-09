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
    public class OrderPrepStatus : BaseEntity
    {
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public long OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        [Column(TypeName = "date")]
        public DateOnly? OrderDate { get; set; }
        public long? AreaId { get; set; }
        public PreparationArea? Area { get; set; }
        public long? KOTDeviceId { get; set; }
        public KitchenDevice? KOTDevice { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Draft;
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
