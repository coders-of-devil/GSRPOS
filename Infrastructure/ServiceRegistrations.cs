using Applications.Interfaces.Common;
using Applications.Interfaces.CustomerInterfaces;
using Applications.Interfaces.MenuInterfaces;
using Applications.Interfaces.TableInterfaces;
using Applications.Interfaces.UserInterfaces;
using Infrastructure.Implementations.Common;
using Infrastructure.Implementations.CustomerImplementations;
using Infrastructure.Implementations.MenuImplementations;
using Infrastructure.Implementations.TableImplementations;
using Infrastructure.Implementations.UserImplementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionGroupService, PermissionGroupService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPreparationAreaService, PreparationAreaService>();
            services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuItemTypeService, MenuItemTypeService>();
            services.AddScoped<IMenuItemPricingService, MenuItemPricingService>();
            services.AddScoped<IPricelistService, PricelistService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IDiningAreaService, DiningAreaService>();
            services.AddScoped<IDiningTableService, TableService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<ITablePriceService, TablePriceService>();
            services.AddScoped<IAreaWaiterService, AreaWaiterService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();

            return services;
        }
    }
}
