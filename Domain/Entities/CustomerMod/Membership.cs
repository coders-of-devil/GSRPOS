using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CustomerMod
{
    public class Membership : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountRate { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal RewardMultiplier { get; set; } = 1m;
        public bool FreeDelivery { get; set; } = false;
    }
}
