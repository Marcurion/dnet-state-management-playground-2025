using Application.StateManagement.Generic;
using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.Pipeline;

public class SynchronizedPipelineRequest<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<IAppState>>
where TRequest : SynchronizedRequest
{
    private static readonly SemaphoreSlim _mutex = new(1, 1);


    public async Task<ErrorOr<IAppState>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<IAppState>> next, CancellationToken cancellationToken)
    {
//        if (_mutex.CurrentCount == 0) // not thread-safe
  //          throw new Exception("Testing concurrent access");
        
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            return await next();
        }
        finally
        {
            _mutex.Release();
        }

    }
}
