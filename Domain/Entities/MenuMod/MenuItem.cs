using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public MenuCategory? Category { get; set; }
        public string StockKeepingUnit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxRate { get; set; } = 5;
        public string Barcode { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public int ExpectedPreparationTime { get; set; } = 0;
        [ForeignKey("PreparationArea")]
        public long PreparationAreaId { get; set; }
        public PreparationArea? PreparationArea { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAvailableToday { get; set; } = true;
    }
}
