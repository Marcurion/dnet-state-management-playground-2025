using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1RequestHandler : IRequestHandler<SetAppState1Request, IAppState1>
{
    public async Task<IAppState1> Handle(SetAppState1Request request, CancellationToken cancellationToken)
    {
        return request.NewState;
    }
}