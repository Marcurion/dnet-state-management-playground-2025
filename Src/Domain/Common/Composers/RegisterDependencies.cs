using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddSingleton<IAppStateWrapper, AppStateWrapper>();
        services.AddSingleton<IAppState1Wrapper, AppState1Wrapper>();
        services.AddSingleton<IAppState, AppState>();
        services.AddSingleton<IAppState1, AppState1>();
        return services;
    }
    
}