using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Specific;

public class GetAppState2RequestHandler : IRequestHandler<GetAppState2Request, IAppState2>

{
    public async Task<IAppState2> Handle(GetAppState2Request request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState;
    }
}