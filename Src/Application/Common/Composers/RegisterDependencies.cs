using Application.Common.PipelineBehaviors;
using Application.StateManagement.Common;
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
        var stringWrapper =new AppStateWrapper<List<string>>(
            services.BuildServiceProvider().GetRequiredService<IAppState<List<string>>>() as AppState<List<string>>);
        var intWrapper =new AppStateWrapper<List<int>>(
            services.BuildServiceProvider().GetRequiredService<IAppState<List<int>>>() as AppState<List<int>>);
        services.AddSingleton<IAppStateWrapper<List<string>>>(stringWrapper);
        services.AddSingleton<AppStateWrapper<List<string>>>(stringWrapper);
        services.AddSingleton<IAppStateWrapper<List<int>>>(intWrapper);
        services.AddSingleton<AppStateWrapper<List<int>>>(intWrapper);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        // Notable: the amount of registered Transients could be reduced down to 2 per distinct state,
        // if we would unify the getter and setter pairs for pipelines, requests and handlers and use one pipeline step for states 
        services
            .AddTransient<IRequestHandler<GetAppStateRequest<List<string>>, AppState<List<string>>>,
                GetAppStateRequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<SetAppStateRequest<List<string>>, AppState<List<string>>>,
                SetAppStateRequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<GetAppStateRequest<List<int>>, AppState<List<int>>>,
                GetAppStateRequestHandler<List<int>>>();
        services
            .AddTransient<IRequestHandler<SetAppStateRequest<List<int>>, AppState<List<int>>>,
                SetAppStateRequestHandler<List<int>>>();
        services.AddTransient(typeof(IPipelineBehavior<GetAppStateRequest<List<string>>, AppState<List<string>>>), typeof(AppStatePipelineBehaviour<AppStateRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<string>>, AppState<List<string>>>), typeof(AppStatePipelineBehaviour<AppStateModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<string>>, AppState<List<string>>>), typeof(AppStateModificationPipelineBehaviour<AppStateModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<GetAppStateRequest<List<int>>, AppState<List<int>>>), typeof(AppStatePipelineBehaviour<AppStateRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<int>>, AppState<List<int>>>), typeof(AppStatePipelineBehaviour<AppStateModificationRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppStateRequest<List<int>>, AppState<List<int>>>), typeof(AppStateModificationPipelineBehaviour<AppStateModificationRequest<List<int>>, List<int>>));

        return services;
    }
}