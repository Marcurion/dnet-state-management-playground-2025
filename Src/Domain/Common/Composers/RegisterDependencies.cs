using Domain.States;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Common.Composers;


public static class RegisterDependencies
{

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        var stringState = new AppState<List<string>>(["Red", "Green", "Blue"]);
        var intState = new AppState<List<int>>([1, 2, 3]);
        services.AddSingleton<IAppState<List<string>>>(stringState);
        services.AddSingleton<IAppState<List<int>>>(intState);
        return services;
    }
    
}