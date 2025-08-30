using Applications.Interfaces.Common;
using Domain.Entities.Common;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common
{
    public class SettingsService(AppDbContext context, IDateService dateService) : ISettingsService
    {

        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;

        public async Task<Setting> GetSettingAsync(int id)
        {
            if (id <= 0)
                throw new ApplicationException("Settings id is empty.");

            var setting = await _context.Settings.Where(e => e.Id == id).FirstOrDefaultAsync()
                ?? throw new ApplicationException($"setting with id : {id} not found.");
            return setting;
        }

        public async Task UpdateSettingAsync(Setting setting)
        {
            if (setting.Id <= 0)
                throw new ApplicationException("settings id is null");

            var currentSetting = await _context.Settings.Where(e => e.Id == setting.Id).FirstOrDefaultAsync()
                ?? throw new ApplicationException($"setting with id : {setting.Id} not found.");

            currentSetting.IsTaxable = setting.IsTaxable;
            currentSetting.IsTaxExclusive = setting.IsTaxExclusive;
            currentSetting.IsTaxInclusive = setting.IsTaxInclusive;
            currentSetting.IsMultiplePriceForItem = setting.IsMultiplePriceForItem;
            currentSetting.TaxRate = setting.TaxRate;

            _context.Settings.Update(currentSetting);
            await _context.SaveChangesAsync();
        }
    }
}
