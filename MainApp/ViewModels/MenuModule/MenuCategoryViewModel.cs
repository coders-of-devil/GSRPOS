using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class MenuCategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsParentCategory { get; set; }
        public long ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public int DisplayOrder { get; set; }
        public string IconPath { get; set; }
        public bool IsSelected { get; set; }
    }
}
