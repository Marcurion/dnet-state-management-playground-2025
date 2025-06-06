using Application.StateManagement.AppState1.Generic;
using Application.StateManagement.Common;
using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Pipeline;

public class AppState1PipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, IAppState1>
    where TRequest : AppState1Request
{
    private static readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly IAppState1Wrapper _wrapperSingleton;

    public AppState1PipelineBehaviour(IAppState1Wrapper wrapperSingleton)
    {
        _wrapperSingleton = wrapperSingleton;
    }


    public async Task<IAppState1> Handle(TRequest request, RequestHandlerDelegate<IAppState1> next,
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