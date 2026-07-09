using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application;

namespace WorldRank;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // All strategies are registered under the same interface. The caller resolves
        // them as a collection and picks the one whose Operation matches - no factory.
        services.AddSingleton<IWalletStrategy, AddFundsWallet>();
        services.AddSingleton<IWalletStrategy, SubtractFunds>();
        services.AddSingleton<IWalletStrategy, ForceSubtractFunds>();
        return services;
    }
}