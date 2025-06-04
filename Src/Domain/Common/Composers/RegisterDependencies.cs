using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddSingleton<IAppStateWrapper, AppStateWrapper>();
        services.AddSingleton<IAppState, AppState>();
        return services;
    }
    
}