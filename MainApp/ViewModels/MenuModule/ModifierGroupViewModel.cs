using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class ModifierGroupViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public int MaxSelectable { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
