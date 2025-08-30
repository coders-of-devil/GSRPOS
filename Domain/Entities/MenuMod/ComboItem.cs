using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class ComboItem : BaseEntity
    {
        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public MenuCategory? Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public string ImagePath { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
