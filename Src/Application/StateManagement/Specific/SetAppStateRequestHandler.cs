using Domain.States;
using MediatR;

namespace Application.StateManagement.Specific;

public class SetAppStateRequestHandler<T> : IRequestHandler<SetAppStateRequest<T>, AppState<T>>
{

    public async Task<AppState<T>> Handle(SetAppStateRequest<T> request, CancellationToken cancellationToken)
    {
        
        return request.NewState;
    }
}