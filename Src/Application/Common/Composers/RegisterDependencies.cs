using Application.Common.PipelineBehaviors;
using Application.StateManagement.Generic;
using Application.StateManagement.Pipeline;
using Application.StateManagement.Specific;
using Domain.States;
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
        // Notable: the amount of registered Transients could be reduced down to 2 per distinct state,
        // if we would unify the getter and setter pairs for pipelines, requests and handlers and use one pipeline step for states 
        services
            .AddTransient<IRequestHandler<GetAppStateRequest<List<string>>, IAppState<List<string>>>,
                GetAppStateRequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<SetAppStateRequest<List<string>>, IAppState<List<string>>>,
                SetAppStateRequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<GetAppStateRequest<List<int>>, IAppState<List<int>>>,
                GetAppStateRequestHandler<List<int>>>();
        services
            .AddTransient<IRequestHandler<SetAppStateRequest<List<int>>, IAppState<List<int>>>,
                SetAppStateRequestHandler<List<int>>>();
        services.AddTransient(typeof(IPipelineBehavior<GetAppStateRequest<List<string>>, IAppState<List<string>>>), typeof(AppStatePipelineBehaviour<AppStateRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<string>>, IAppState<List<string>>>), typeof(AppStatePipelineBehaviour<AppStateModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<string>>, IAppState<List<string>>>), typeof(AppStateModificationPipelineBehaviour<AppStateModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<GetAppStateRequest<List<int>>, IAppState<List<int>>>), typeof(AppStatePipelineBehaviour<AppStateRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<int>>, IAppState<List<int>>>), typeof(AppStatePipelineBehaviour<AppStateModificationRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<int>>, IAppState<List<int>>>), typeof(AppStateModificationPipelineBehaviour<AppStateModificationRequest<List<int>>, List<int>>));

        return services;
    }
}