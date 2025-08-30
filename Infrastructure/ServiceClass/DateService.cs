using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceClass
{
    public class DateService : IDateService
    {
        public DateTime Now => DateTime.UtcNow.AddHours(4);
    }
}
