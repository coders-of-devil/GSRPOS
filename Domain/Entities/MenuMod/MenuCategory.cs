using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsParentCategory { get; set; } = false;
        [ForeignKey("ParentCategory")]
        public long? ParentCategoryId { get; set; }
        public MenuCategory? ParentCategory { get; set; }
        public int DisplayOrder { get; set; }
        public string IconPath { get; set; } = string.Empty;
    }
}
