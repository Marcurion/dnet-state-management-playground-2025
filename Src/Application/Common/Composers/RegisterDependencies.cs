using Application.Common.PipelineBehaviors;
using Application.StateManagement.AppState1.Generic;
using Application.StateManagement.AppState1.Pipeline;
using Application.StateManagement.AppState1.Specific;
using Application.StateManagement.AppState2.Pipeline;
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
        services
            .AddTransient<IRequestHandler<GetAppState1Request<List<string>>, IAppState1<List<string>>>,
                GetAppState1RequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<SetAppState1Request<List<string>>, IAppState1<List<string>>>,
                SetAppState1RequestHandler<List<string>>>();
        services
            .AddTransient<IRequestHandler<GetAppState1Request<List<int>>, IAppState1<List<int>>>,
                GetAppState1RequestHandler<List<int>>>();
        services
            .AddTransient<IRequestHandler<SetAppState1Request<List<int>>, IAppState1<List<int>>>,
                SetAppState1RequestHandler<List<int>>>();
        services.AddTransient(typeof(IPipelineBehavior<GetAppState1Request<List<string>>, IAppState1<List<string>>>), typeof(AppState1PipelineBehaviour<AppState1Request<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppState1Request<List<string>>, IAppState1<List<string>>>), typeof(AppState1PipelineBehaviour<AppState1ModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppState1Request<List<string>>, IAppState1<List<string>>>), typeof(AppState1ModificationPipelineBehaviour<AppState1ModificationRequest<List<string>>, List<string>>));
        services.AddTransient(typeof(IPipelineBehavior<GetAppState1Request<List<int>>, IAppState1<List<int>>>), typeof(AppState1PipelineBehaviour<AppState1Request<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppState1Request<List<int>>, IAppState1<List<int>>>), typeof(AppState1PipelineBehaviour<AppState1ModificationRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<SetAppState1Request<List<int>>, IAppState1<List<int>>>), typeof(AppState1ModificationPipelineBehaviour<AppState1ModificationRequest<List<int>>, List<int>>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState2PipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AppState2ModificationPipelineBehaviour<,>));

        return services;
    }
}