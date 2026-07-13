using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Data;
using WorldRank.Infrastructure.Repositories;

namespace WorldRank.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
	{
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var useDatabase = !string.IsNullOrWhiteSpace(connectionString);
        if (useDatabase)
        {
            services.AddDbContext<WorldRankDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IPlayerRepository, DBPlayerRepo>();
            services.AddScoped<IWalletRepository, DBWalletRepo>();
        }
        else
        {
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
            services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
        }
        return services;
	}
}
