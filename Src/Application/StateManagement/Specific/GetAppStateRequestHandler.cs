using Domain.States;
using MediatR;

namespace Application.StateManagement.Specific;

public class GetAppStateRequestHandler<T> : IRequestHandler<GetAppStateRequest<T>, AppState<T>>

{

    public async Task<AppState<T>> Handle(GetAppStateRequest<T> request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState;
    }
}