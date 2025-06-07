using Application.StateManagement.Common;
using Application.StateManagement.Generic;
using Domain.States;
using MediatR;

namespace Application.StateManagement.Pipeline;

/// <summary>
/// Generic pipeline behavior for state modification requests.
/// Provides optimistic concurrency control and change detection.
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TState">The state type</typeparam>
public class AppStateModificationPipelineBehaviour<TRequest, TState>
    : IPipelineBehavior<TRequest, TState>
    where TRequest : AppStateModificationRequest<TState>
    where TState : class, IAppState<TState>
{
    private readonly IAppStateWrapper<TState> _stateWrapper;

    public AppStateModificationPipelineBehaviour(IAppStateWrapper<TState> stateWrapper)
    {
        _stateWrapper = stateWrapper;
    }

    public async Task<TState> Handle(TRequest request, RequestHandlerDelegate<TState> next,
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