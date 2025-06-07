using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddSingleton<IAppStateWrapper<List<string>>, AppState1Wrapper>();
        services.AddSingleton<IAppStateWrapper<List<int>>, AppState2Wrapper>();
        services.AddSingleton<IAppState<List<string>>, AppState1>();
        services.AddSingleton<IAppState<List<int>>, AppState2>();
        return services;
    }
    
}