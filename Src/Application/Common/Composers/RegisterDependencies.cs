using Application.Common.PipelineBehaviors;
using Application.StateManagement.AppState1.Pipeline;
using Application.StateManagement.AppState2.Pipeline;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Composers;

public static class RegisterDependencies
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddSingleton<MyManager>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState1PipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState1ModificationPipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState2PipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState2ModificationPipelineBehaviour<,>));

        return services;
    }
}