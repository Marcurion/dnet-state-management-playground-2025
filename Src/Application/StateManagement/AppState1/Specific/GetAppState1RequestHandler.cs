using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class GetAppState1RequestHandler : IRequestHandler<GetAppState1Request, Domain.States.AppState1>
{
    public async Task<Domain.States.AppState1> Handle(GetAppState1Request request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState!;
    }
}