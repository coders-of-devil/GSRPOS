using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.TableModule
{
    public class DiningAreaViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsReservable { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; } = false;

        public List<AreaWaiterViewModel> Waiters { get; set; } = new();
    }
}
