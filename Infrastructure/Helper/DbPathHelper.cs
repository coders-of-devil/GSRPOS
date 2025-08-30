using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public static class DbPathHelper
    {
        public static string GetDbPath()
        {
#if ANDROID || IOS || WINDOWS
            return Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");
#else
            return Path.Combine(AppContext.BaseDirectory, "restaurant.db");
#endif
        }
    }
}
