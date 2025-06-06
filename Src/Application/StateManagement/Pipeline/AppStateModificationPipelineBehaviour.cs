using Application.StateManagement.Generic;
using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.Pipeline;

public class AppStateModificationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, ErrorOr<IAppState>>
    where TRequest : AppStateModificationRequest
// where TInner : IAppState
{
    private readonly IAppStateWrapper _stateWrapper;

    public AppStateModificationPipelineBehaviour(IAppStateWrapper stateWrapper)
    {
        _stateWrapper = stateWrapper;
    }


    public async Task<ErrorOr<IAppState>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<IAppState>> next,
        CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentException("Request must not be null");

        if (request.LastStateHash == default)
            throw new ArgumentException("You must provide a valid hash for a modification operation");

        if (request.InternalLatestState == null)
            throw new NullReferenceException(
                "Internal latest state should be populated by the pipeline, please check the order pipeline behaviours are registered");

        if (request.InternalLatestState.GetHashCode() != request.LastStateHash)
            throw new ArgumentException(
                "The provided hash differs from the latest one, possible write conflict, aborting...");

        var preOperationHash = _stateWrapper.CurrentState.GetHashCode();

        var operationResult = await next();

        if (operationResult.Value.GetHashCode() != preOperationHash)
        {
            _stateWrapper.CurrentState = operationResult.Value;
            request.InternalLatestState.AppStateChanged?.Invoke(operationResult.Value);
        }

        return operationResult;
    }
}