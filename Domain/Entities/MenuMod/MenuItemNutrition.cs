using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuItemNutrition : BaseEntity
    {
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public string Calories { get; set; } = string.Empty;
        public string Protien { get; set; } = string.Empty; 
        public string Fat { get; set; } = string.Empty;
        public string Sugar { get; set; } = string.Empty;
        public string Sodium { get; set; } = string.Empty;
        public string CarboHydrates { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
