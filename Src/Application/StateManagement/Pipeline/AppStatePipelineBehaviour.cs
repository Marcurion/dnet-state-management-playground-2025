using System.Diagnostics;
using Application.StateManagement.Generic;
using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.Pipeline;

  
    
    // public class AppStatePipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    public class AppStatePipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<IAppState>>
    where TRequest : AppStateRequest
    {
        private  IAppStateWrapper _wrapperSingleton;
    public AppStatePipelineBehaviour(IAppStateWrapper wrapperSingleton)
    {
        _wrapperSingleton = wrapperSingleton;
    }


    // public async Task<ErrorOr<TInner>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<TInner>> next, CancellationToken cancellationToken)
    // {
    //     
    //     if (request == null)
    //         throw new ArgumentException("Request must not be null");
    //     
    //     if (request.InternalLatestState != null)
    //         throw new ArgumentException("Internal state should not be populated by the user");
    //
    //     request.InternalLatestState = _stateSingleton;
    //     Debug.Assert(_stateSingleton != null);
    //     // return await next();
    //     var operationResult =  await next();
    //
    //     // if (operationResult.Value.GetHashCode() != _stateSingleton.GetHashCode())
    //     //     // Write State back
    //     //     _stateSingleton = operationResult.Value;
    //     
    //     Debug.Assert(operationResult.Value != null);
    //
    //     return operationResult;
    // }

    // public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    // {
    //    
    //     
    //     if (request == null)
    //         throw new ArgumentException("Request must not be null");
    //     
    //     if (request.InternalLatestState != null)
    //         throw new ArgumentException("Internal state should not be populated by the user");
    //
    //     request.InternalLatestState = _stateSingleton;
    //     return await next();
    //
    // }
    public async Task<ErrorOr<IAppState>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<IAppState>> next, CancellationToken cancellationToken)
    {
        
        if (request == null)
            throw new ArgumentException("Request must not be null");
        
        if (request.InternalLatestState != null)
            throw new ArgumentException("Internal state should not be populated by the user");
    
        request.InternalLatestState = _wrapperSingleton.CurrentState;
        // return await next();
        var operationResult =  await next();

        // if (operationResult.Value.GetHashCode() != _wrapperSingleton.CurrentState.GetHashCode())
        // {
        //      // Write State back
        //       _wrapperSingleton.CurrentState = operationResult.Value;
        // }

        
        return operationResult;

    }
    }