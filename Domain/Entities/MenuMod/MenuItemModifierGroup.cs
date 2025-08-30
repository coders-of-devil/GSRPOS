using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuItemModifierGroup : BaseEntity
    {
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        [ForeignKey("ModifierGroup")]
        public long ModifierGroupId { get; set; }
        public ModifierGroup? ModifierGroup { get; set; }
    }
}
