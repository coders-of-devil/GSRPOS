using Domain.Services;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Helper;
using Infrastructure.Seed;
using Infrastructure.ServiceClass;
using MainApp.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MainApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            var dbPath = DbPathHelper.GetDbPath();
            //if (File.Exists(dbPath))
            //{
            //    File.Delete(dbPath); // 🔥 Deletes the SQLite DB file
            //}
            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite($"Data Source={dbPath}"));
            builder.Services.AddScoped<IIdGeneratorService, IdGeneratorService>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<IDateService, DateService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddTransient<RuntimeSeeder>();
            builder.Services.AddApplicationServices();
            builder.Services.AddSingleton<FileLogger>();

            // Set license for QuestPDF
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // Build the app
            var app = builder.Build();

            // Get NotificationService to reuse in exception logging
            var serviceProvider = app.Services;

            // Global exception handler
            var fileLogger = serviceProvider.GetRequiredService<FileLogger>();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    var message = $"[CRITICAL] Unhandled Exception: {ex.Message}\n{ex.StackTrace}";
                    Console.WriteLine(message);

                    try
                    {
                        var notifier = serviceProvider.GetRequiredService<NotificationService>();
                        notifier.NotifyError(ex, "AppDomain");
                        fileLogger.Log(message, "AppDomain");
                    }
                    catch
                    {
                        // Avoid throwing from global handler
                        Console.WriteLine("[ERROR] Unable to notify from AppDomain exception");
                        fileLogger.Log("❌ Error in error handler", "AppDomain");
                    }
                }
            };

            return app;
        }
    }
}
