using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddSingleton<IAppState1Wrapper<List<string>>, AppState1Wrapper>();
        services.AddSingleton<IAppState1Wrapper<List<int>>, AppState2Wrapper>();
        services.AddSingleton<IAppState1<List<string>>, AppState1>();
        services.AddSingleton<IAppState1<List<int>>, AppState2>();
        return services;
    }
    
}