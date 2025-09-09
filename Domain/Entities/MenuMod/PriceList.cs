using Domain.Entities.Common;
using Domain.Entities.CustomerMod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class PriceList : BaseEntity
    {
        [ForeignKey("PricelistTypes")]
        public long TypeId { get; set; }
        public PricelistTypes? PricelistTypes { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsTemporary { get; set; } = false;
        public DateTime EffectiveFrom { get; set; } = DateTime.MinValue;
        public DateTime EffectiveTo { get; set;} = DateTime.MinValue;
        public string Notes { get; set; } = string.Empty;
        //public bool IsCustomerRelated { get; set; }
        //public long CustomerId { get; set; }
        //public Customer? Customer { get; set; }
    }
}
