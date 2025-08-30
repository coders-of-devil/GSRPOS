using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.TableModule
{
    public class TablePriceViewModel
    {
        public long Id { get; set; }
        public long TableId { get; set; }
        public string DayType { get; set; } = string.Empty;
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public decimal PricePerHour { get; set; }
        public bool IsActive { get; set; }
    }
}
