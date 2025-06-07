using Domain.States;
using MediatR;

namespace Application.StateManagement.Specific;

public class SetAppStateRequestHandler<T> : IRequestHandler<SetAppStateRequest<T>, IAppState<T>>
{

    public async Task<IAppState<T>> Handle(SetAppStateRequest<T> request, CancellationToken cancellationToken)
    {
        
        return request.NewState;
    }
}