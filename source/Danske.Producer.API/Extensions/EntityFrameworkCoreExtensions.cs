using Danske.Producer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Danske.Producer.API.Extensions
{
    internal static class EntityFrameworkCoreExtensions
    {
        internal static IServiceCollection AddTaxesDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<TaxesDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
                options.UseSqlServer(connectionString, o => o.MigrationsAssembly("Danske.Producer.Infrastructure.Migrations").CommandTimeout(60));
            });
        }
    }
}