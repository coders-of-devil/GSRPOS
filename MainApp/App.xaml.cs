using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace MainApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _ = RunStartupSeederAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MainApp" };
        }

        private async Task RunStartupSeederAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();

            var seeder = scope.ServiceProvider.GetRequiredService<RuntimeSeeder>();
            await seeder.SeedAsync();
        }
    }
}
