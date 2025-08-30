using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ItemPricelistDTO
    {
        public long Id { get; set; }
        public long PriceListId { get; set; }
        public string PriceListName { get; set; } = string.Empty;
        public long MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;

    }
}
