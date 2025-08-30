using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;
using Infrastructure.Helper;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var dbPath = DbPathHelper.GetDbPath();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
    })
    .Build();

host.Run();
