using Application.StateManagement.Common;
using Application.StateManagement.Generic;
using Domain.States;
using MediatR;

namespace Application.StateManagement.Pipeline;

public class AppStatePipelineBehaviour<TRequest, T> : IPipelineBehavior<TRequest, IAppState<T>>
    where TRequest : AppStateRequest<T>
{
    private static readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly IAppStateWrapper<T> _wrapperSingleton;

    public AppStatePipelineBehaviour(IAppStateWrapper<T> wrapperSingleton)
    {
        _wrapperSingleton = wrapperSingleton;
    }


    public async Task<IAppState<T>> Handle(TRequest request, RequestHandlerDelegate<IAppState<T>> next, CancellationToken cancellationToken)
    {
        // if (_mutex.CurrentCount == 0) // not thread-safe, for debugging only
        //     throw new MutexBusyException(); 

        await _mutex.WaitAsync(cancellationToken);
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
            _mutex.Release();
        }
    }
}