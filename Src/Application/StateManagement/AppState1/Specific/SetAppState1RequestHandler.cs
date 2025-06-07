using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1RequestHandler : IRequestHandler<SetAppState1Request, Domain.States.AppState1>
{
    public async Task<Domain.States.AppState1> Handle(SetAppState1Request request, CancellationToken cancellationToken)
    {
        return request.NewState;
    }
}