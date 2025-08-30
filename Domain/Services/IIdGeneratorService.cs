using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IIdGeneratorService
    {
        Task<long> GenerateNextId<T>() where T : class;
        long GetLastUsedId<T>();
    }
}
