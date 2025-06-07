using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Specific;

public class GetAppState2RequestHandler : IRequestHandler<GetAppState2Request, Domain.States.AppState2>
{
    public async Task<Domain.States.AppState2> Handle(GetAppState2Request request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState!;
    }
}