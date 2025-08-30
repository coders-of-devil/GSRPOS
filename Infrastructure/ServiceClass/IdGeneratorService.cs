using Domain.Entities.Common;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceClass
{
    public class IdGeneratorService(AppDbContext context) : IIdGeneratorService
    {
        private readonly Dictionary<string, long> _lastIds = new();
        private readonly AppDbContext _context = context;

        public async Task<long> GenerateNextId<T>() where T : class
        {
            var dbSet = _context.Set<T>();

            var maxId = await dbSet
                .Select(e => EF.Property<long>(e, "Id")) 
                .OrderByDescending(id => id)
                .FirstOrDefaultAsync();

            return maxId + 1;
        }

        public long GetLastUsedId<T>()
        {
            string key = typeof(T).Name;
            return _lastIds.ContainsKey(key) ? _lastIds[key] : 0;
        }
    }
}
