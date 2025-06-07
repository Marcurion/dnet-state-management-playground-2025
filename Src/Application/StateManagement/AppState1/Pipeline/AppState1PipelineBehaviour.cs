using Application.StateManagement.AppState1.Generic;
using Application.StateManagement.Common;
using Domain.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.StateManagement.AppState1.Pipeline;

public class AppState1PipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private static readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly IServiceProvider _serviceProvider;

    public AppState1PipelineBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static bool IsAppState1Request(Type requestType)
    {
        var currentType = requestType;
        while (currentType != null)
        {
            if (currentType.IsGenericType && 
                currentType.GetGenericTypeDefinition() == typeof(AppState1Request<>))
                return true;
            currentType = currentType.BaseType;
        }
        return false;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        // Only process AppState1Request<T> types and their derived types
        if (IsAppState1Request(request.GetType()))
        {
            
            await _mutex.WaitAsync(cancellationToken);
            try
            {
                if (request == null)
                    throw new ArgumentException("Request must not be null");

                // Use reflection to get the generic type T and wrapper
                var requestType = request.GetType();
                var genericArgs = requestType.BaseType?.GetGenericArguments();
                if (genericArgs?.Length > 0)
                {
                    var itemType = genericArgs[0];
                    var wrapperType = typeof(IAppState1Wrapper<>).MakeGenericType(itemType);
                    var wrapper = _serviceProvider.GetService(wrapperType);
                    
                    if (wrapper != null)
                    {
                        var internalStateProperty = request.GetType().BaseType?.GetProperty("InternalLatestState", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (internalStateProperty != null)
                        {
                            var currentStateValue = internalStateProperty.GetValue(request);
                            if (currentStateValue != null)
                                throw new InternalStatePrepopulatedException();

                            var currentStateProperty = wrapper.GetType().GetProperty("CurrentState");
                            var currentState = currentStateProperty?.GetValue(wrapper);
                            internalStateProperty.SetValue(request, currentState);
                        }
                    }
                }
                return await next();
            }
            finally
            {
                _mutex.Release();
            }
        }
        
        return await next();
    }
}