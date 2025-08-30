using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuItemType : BaseEntity
    {
        public long ItemId { get; set; }
        public long PricelistId { get; set; }
        public string TypeName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        
    }
}
