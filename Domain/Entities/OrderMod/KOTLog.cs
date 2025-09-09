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
    public class KOTLog : BaseEntity
    {
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        [Column(TypeName = "date")]
        public DateOnly? OrderDate { get; set; }
        public long OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public long KOTDeviceId { get; set; }
        public KitchenDevice? KOTDevice { get; set; }
        public bool IsTicketRaised { get; set; } = false;
        public DateTime? PrintedAt { get; set; }
        public string ErrorsOccured { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
