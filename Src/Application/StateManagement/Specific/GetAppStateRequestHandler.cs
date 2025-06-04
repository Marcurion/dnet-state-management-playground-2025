using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.Specific;

public class GetAppStateRequestHandler: IRequestHandler<GetAppStateRequest, ErrorOr<IAppState>>
{
    public async Task<ErrorOr<IAppState>> Handle(GetAppStateRequest request, CancellationToken cancellationToken)
    {
        return request.InternalLatestState.ToErrorOr();
    }
}