using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Specific;

public class SetAppState2RequestHandler : IRequestHandler<SetAppState2Request, Domain.States.AppState2>
{
    public async Task<Domain.States.AppState2> Handle(SetAppState2Request request, CancellationToken cancellationToken)
    {
        return request.NewState;
    }
}