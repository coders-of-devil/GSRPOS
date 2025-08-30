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
    public class RewardEarning : BaseEntity
    {
        [Required]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public long? OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Points { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal MultiplerApplied { get; set; } = 1m;
        public string Reason { get; set; } = string.Empty;
        public DateTime? EarnedAt { get; set; }
    }
}
