using Domain.States;
using MediatR;

namespace Application.StateManagement.Specific;

public class GetAppStateRequestHandler<T> : IRequestHandler<GetAppStateRequest<T>, IAppState<T>>

{

    public async Task<IAppState<T>> Handle(GetAppStateRequest<T> request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState;
    }
}