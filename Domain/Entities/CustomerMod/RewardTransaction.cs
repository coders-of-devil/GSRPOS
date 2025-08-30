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
    public class RewardTransaction : BaseEntity
    {
        [Required]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public long? OrderId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PointsEarned { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal PointsUsed { get; set; } = 0m;
        [Column(TypeName = "date")]
        public DateOnly? TransactionDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public bool IsCancelled { get; set; } = false;
    }
}
