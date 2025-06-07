using Application.StateManagement.Common;
using Application.StateManagement.Generic;
using Domain.States;
using MediatR;

namespace Application.StateManagement.Pipeline;

/// <summary>
/// Generic pipeline behavior for all app state requests.
/// Provides thread synchronization and state injection.
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TState">The state type</typeparam>
public class AppStatePipelineBehaviour<TRequest, TState> : IPipelineBehavior<TRequest, TState>
    where TRequest : AppStateRequest<TState>
    where TState : class, IAppState<TState>
{
    private static readonly Dictionary<Type, SemaphoreSlim> _mutexes = new();
    private readonly IAppStateWrapper<TState> _wrapperSingleton;

    public AppStatePipelineBehaviour(IAppStateWrapper<TState> wrapperSingleton)
    {
        _wrapperSingleton = wrapperSingleton;
    }

    public async Task<TState> Handle(TRequest request, RequestHandlerDelegate<TState> next,
        CancellationToken cancellationToken)
    {
        // Get or create mutex for this state type
        var mutex = GetMutexForStateType<TState>();
        
        await mutex.WaitAsync(cancellationToken);
        try
        {
            if (request == null)
                throw new ArgumentException("Request must not be null");

            if (request.InternalLatestState != null)
                throw new InternalStatePrepopulatedException();

            request.InternalLatestState = _wrapperSingleton.CurrentState;

            return await next();
        }
        finally
        {
            mutex.Release();
        }
    }

    private static SemaphoreSlim GetMutexForStateType<T>()
    {
        var stateType = typeof(T);
        if (!_mutexes.TryGetValue(stateType, out var mutex))
        {
            lock (_mutexes)
            {
                if (!_mutexes.TryGetValue(stateType, out mutex))
                {
                    mutex = new SemaphoreSlim(1, 1);
                    _mutexes[stateType] = mutex;
                }
            }
        }
        return mutex;
    }
}