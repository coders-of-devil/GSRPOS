using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.TableModule
{
    public class AreaWaiterViewModel
    {
        public long Id { get; set; }
        public long WaiterId { get; set; }
        public string WaiterName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime AssignedOn { get; set; }
    }
}
