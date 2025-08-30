using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class PreparationArea : BaseEntity
    {
        public string Name { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
