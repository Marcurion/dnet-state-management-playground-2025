using Application.StateManagement.AppState2.Generic;
using Application.StateManagement.Common;
using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Pipeline;

public class AppState2PipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, IAppState2>
    where TRequest : AppState2Request
{
    private static readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly IAppState2Wrapper _wrapperSingleton;

    public AppState2PipelineBehaviour(IAppState2Wrapper wrapperSingleton)
    {
        _wrapperSingleton = wrapperSingleton;
    }


    public async Task<IAppState2> Handle(TRequest request, RequestHandlerDelegate<IAppState2> next,
        CancellationToken cancellationToken)
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