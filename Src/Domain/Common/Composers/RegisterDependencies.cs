using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        var stringState = new AppState<List<string>>() { Value = ["Red", "Green", "Blue"] };
        var intState = new AppState<List<int>>() { Value = [1, 2, 3] };
        services.AddSingleton<IAppStateWrapper<List<string>>>(new AppStateWrapper<List<string>>(stringState));
        services.AddSingleton<IAppStateWrapper<List<int>>>(new AppStateWrapper<List<int>>(intState));
        services.AddSingleton<IAppState<List<string>>>(stringState);
        services.AddSingleton<IAppState<List<int>>>(intState);
        return services;
    }
    
}