using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        // Register concrete state types
        services.AddSingleton<AppState1>();
        services.AddSingleton<AppState2>();
        
        // Register interfaces that delegate to concrete types
        services.AddSingleton<IAppState1>(provider => provider.GetRequiredService<AppState1>());
        services.AddSingleton<IAppState2>(provider => provider.GetRequiredService<AppState2>());
        
        // Register wrappers
        services.AddSingleton<IAppState1Wrapper, AppState1Wrapper>();
        services.AddSingleton<IAppState2Wrapper, AppState2Wrapper>();
        
        return services;
    }
    
}