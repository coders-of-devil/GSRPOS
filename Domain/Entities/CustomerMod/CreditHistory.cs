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
    public class CreditHistory : BaseEntity
    {
        [Required]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public long? OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; } = 0m;
        public string Reason { get; set; } = string.Empty;
        public DateTime? OrderCreatedAt { get; set; }
        public DateTime? CreditMarkedAt { get; set; }
    }
}
