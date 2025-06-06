using Application.StateManagement.AppState2.Generic;
using Application.StateManagement.Common;
using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Pipeline;

public class AppState2ModificationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, IAppState2>
    where TRequest : AppState2ModificationRequest
// where TInner : IAppState
{
    private readonly IAppState2Wrapper _stateWrapper;

    public AppState2ModificationPipelineBehaviour(IAppState2Wrapper stateWrapper)
    {
        _stateWrapper = stateWrapper;
    }


    public async Task<IAppState2> Handle(TRequest request, RequestHandlerDelegate<IAppState2> next,
        CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentException("Request must not be null");

        if (request.LastStateHash == default)
            throw new HashNotProvidedException();

        // Internal latest state should be populated by the pipeline, please check the order pipeline behaviours are registered
        if (request.InternalLatestState == null)
            throw new InternalStateNullException();

        if (request.InternalLatestState.GetHashCode() != request.LastStateHash)
            throw new HashOutdatedException();

        var preOperationHash = _stateWrapper.CurrentState.GetHashCode();

        var operationResult = await next();

        if (operationResult.GetHashCode() != preOperationHash)
        {
            _stateWrapper.CurrentState = operationResult;
            _stateWrapper.StateChanged?.Invoke(operationResult);
        }

        return operationResult;
    }
}