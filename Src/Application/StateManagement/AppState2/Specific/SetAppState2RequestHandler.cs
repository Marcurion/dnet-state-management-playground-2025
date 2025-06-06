using Application.StateManagement.Specific;
using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.AppState2.Specific;

public class SetAppState2RequestHandler : IRequestHandler<SetAppState2Request, IAppState2>
{
    public async Task<IAppState2> Handle(SetAppState2Request request, CancellationToken cancellationToken)
    {
        return request.NewState;
    }
}