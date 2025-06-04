using Domain.States;
using MediatR;
using ErrorOr;

namespace Application.StateManagement.Specific;

public class SetAppStateRequestHandler: IRequestHandler<SetAppStateRequest, ErrorOr<IAppState>>
{
    public async Task<ErrorOr<IAppState>> Handle(SetAppStateRequest request, CancellationToken cancellationToken)
    {
        return request.NewState.ToErrorOr();
    }
}