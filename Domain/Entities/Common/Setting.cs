using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class Setting
    {
        public int Id { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsTaxInclusive { get; set; }
        public bool IsTaxExclusive { get; set; }
        public int TaxRate { get; set; }
        public bool IsMultiplePriceForItem { get; set; }
        public bool IsDeliveryPriceChange { get; set; }
    }
}
