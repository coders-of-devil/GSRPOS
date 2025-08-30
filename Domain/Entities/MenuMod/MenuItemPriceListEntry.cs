using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public  class MenuItemPriceListEntry : BaseEntity
    {
        [ForeignKey("PriceList")]
        public long PriceListId { get; set; }
        public PriceList? PriceList { get; set; }
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Notes { get; set; }
    }
}
