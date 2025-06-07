using Application.StateManagement.AppState1.Generic;
using Application.StateManagement.Common;
using Domain.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.StateManagement.AppState1.Pipeline;

public class AppState1ModificationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly IServiceProvider _serviceProvider;

    public AppState1ModificationPipelineBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static bool IsAppState1ModificationRequest(Type requestType)
    {
        var currentType = requestType;
        while (currentType != null)
        {
            if (currentType.IsGenericType && 
                currentType.GetGenericTypeDefinition() == typeof(AppState1ModificationRequest<>))
                return true;
            currentType = currentType.BaseType;
        }
        return false;
    }




    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        // Only process AppState1ModificationRequest<T> types and their derived types
        if (IsAppState1ModificationRequest(request.GetType()))
        {
            
            if (request == null)
                throw new ArgumentException("Request must not be null");

            // Use reflection to get LastStateHash (it's a field, not a property)
            var lastStateHashField = request.GetType().BaseType?.GetField("LastStateHash");
            var lastStateHash = lastStateHashField?.GetValue(request);
            if (lastStateHash != null && lastStateHash.Equals(default(int)))
                throw new HashNotProvidedException();

            
            // Get InternalLatestState
            var internalLatestStateProperty = request.GetType().BaseType?.GetProperty("InternalLatestState", BindingFlags.NonPublic | BindingFlags.Instance);
            var internalLatestState = internalLatestStateProperty?.GetValue(request);
            if (internalLatestState == null)
                throw new InternalStateNullException();

            if (lastStateHash != null && internalLatestState.GetHashCode() != (int)lastStateHash)
                throw new HashOutdatedException();

            // Get wrapper using reflection
            var requestType = request.GetType();
            var genericArgs = requestType.BaseType?.GetGenericArguments();
            if (genericArgs?.Length > 0)
            {
                var itemType = genericArgs[0];
                var wrapperType = typeof(IAppState1Wrapper<>).MakeGenericType(itemType);
                var wrapper = _serviceProvider.GetService(wrapperType);
                
                if (wrapper != null)
                {
                    var currentStateProperty = wrapper.GetType().GetProperty("CurrentState");
                    var preOperationHash = currentStateProperty?.GetValue(wrapper)?.GetHashCode() ?? 0;

                    var operationResult = await next();

                    var newStateHash = operationResult?.GetHashCode() ?? 0;
                    if (newStateHash != preOperationHash)
                    {
                        currentStateProperty?.SetValue(wrapper, operationResult);
                        
                        // Get the StateChanged Action property and invoke it
                        var stateChangedProperty = wrapper.GetType().GetProperty("StateChanged");
                        var stateChangedAction = stateChangedProperty?.GetValue(wrapper);
                        if (stateChangedAction != null)
                        {
                            // Invoke the Action<IAppState1<T>> delegate using reflection
                            var invokeMethod = stateChangedAction.GetType().GetMethod("Invoke");
                            invokeMethod?.Invoke(stateChangedAction, new object[] { operationResult });
                        }
                    }

                    return operationResult;
                }
            }
        }
        
        return await next();
    }
}