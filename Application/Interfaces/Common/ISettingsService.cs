using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Common
{
    public interface ISettingsService
    {
        Task<Setting> GetSettingAsync(int id);
        Task UpdateSettingAsync(Setting setting);
    }
}
