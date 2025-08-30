using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class Modifier : BaseEntity
    {
        [ForeignKey("ModifierGroup")]
        public long ModifierGroupId { get; set; }
        public ModifierGroup? ModifierGroup { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
    }
}
