using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.TableModule
{
    public class TableViewModel
    {
        public long Id { get; set; }
        public long AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public bool IsActive { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsReservable { get; set; }

        public List<SeatViewModel> Seats { get; set; } = new();
        public List<TablePriceViewModel> Prices { get; set; } = new();
        public bool IsSelected { get; set; } = false;
    }

}
