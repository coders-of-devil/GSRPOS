using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuDeliveryPrice : BaseEntity
    {
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        [ForeignKey("Platform")]
        public long PlatformId { get; set; }
        public DeliveryPlatform? Platform { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal CommissionRate { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
