using Application.StateManagement.Specific;
using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1RequestHandler : IRequestHandler<SetAppStateRequest, ErrorOr<IAppState>>
{
    public async Task<ErrorOr<IAppState>> Handle(SetAppStateRequest request, CancellationToken cancellationToken)
    {
        return request.NewState.ToErrorOr();
    }
}