using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class ComboItemDetail : BaseEntity
    {
        public long ComboId { get; set; }
        public ComboItem? Combo { get; set; }
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
    }
}
