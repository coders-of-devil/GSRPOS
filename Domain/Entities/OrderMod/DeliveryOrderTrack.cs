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
    public class DeliveryOrderTrack : BaseEntity
    {
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public DateOnly? OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsOwnDelivery { get; set; } = false;
        public long? DeliveryBoyId { get; set; }
        public User? DeliveryBoy { get; set; }
        public bool IsPartnerDelivery { get; set; } = false;
        public long? DeliveryPartnerId { get; set; }
        public bool IsOrderVoided { get; set; } = false;
        public string Remarks { get; set; } = string.Empty;
        public bool IsAssigned { get; set; } = false;
        public DateTime? AssignedAt { get; set; }
        public bool IsPicked { get; set; } = false;
    }
}
