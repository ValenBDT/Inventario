using System.Net.NetworkInformation;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Inventory.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Persistence
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddContextSqlServer(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionString
        )
        {
            services.AddSqlServer<DataContext>(configuration.GetConnectionString(connectionString));
            return services;
        }

        public static IServiceCollection AddContextSqlite(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionString
        )
        {
            services.AddSqlite<DataContext>(configuration.GetConnectionString(connectionString));
            return services;
        }

        public static IServiceCollection AddRepositories(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();
            services.AddScoped<IInventoryStockRepository, InventoryStockRepository>();
            services.AddScoped<IMovementTypeRepository, MovementTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            return services;
        }
    }
}