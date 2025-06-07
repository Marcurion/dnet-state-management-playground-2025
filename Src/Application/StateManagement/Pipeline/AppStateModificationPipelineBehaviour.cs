using Application.StateManagement.Common;
using Application.StateManagement.Generic;
using Domain.States;
using MediatR;

namespace Application.StateManagement.Pipeline;

public class AppStateModificationPipelineBehaviour<TRequest, T>
    : IPipelineBehavior<TRequest, IAppState<T>>
    where TRequest : AppStateModificationRequest<T>
// where TInner : IAppState
{
    private readonly IAppStateWrapper<T> _stateWrapper;

    public AppStateModificationPipelineBehaviour(IAppStateWrapper<T> stateWrapper)
    {
        _stateWrapper = stateWrapper;
    }




    public async Task<IAppState<T>> Handle(TRequest request, RequestHandlerDelegate<IAppState<T>> next, CancellationToken cancellationToken)
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