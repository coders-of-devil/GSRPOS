using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class PreparationAreaViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSelected { get; set; }
    }
}
