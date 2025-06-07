using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class GetAppState1RequestHandler<T> : IRequestHandler<GetAppState1Request<T>, IAppState1<T>>

{

    public async Task<IAppState1<T>> Handle(GetAppState1Request<T> request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState;
    }
}