using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class GetAppState1RequestHandler : IRequestHandler<GetAppState1Request, IAppState1>

{
    public async Task<IAppState1> Handle(GetAppState1Request request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState;
    }
}