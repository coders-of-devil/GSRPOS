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
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly? DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public long? MembershipId { get; set; }
        public Membership? Membership { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LoyaltyPoints { get; set; } = 0m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal CreditBalance { get; set; } = 0m;
        public string Source { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
