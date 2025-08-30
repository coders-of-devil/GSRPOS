using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.TableModule
{
    public class SeatViewModel
    {
        public long Id { get; set; }
        public long TableId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
    }
}
