using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1RequestHandler<T> : IRequestHandler<SetAppState1Request<T>, IAppState1<T>>
{

    public async Task<IAppState1<T>> Handle(SetAppState1Request<T> request, CancellationToken cancellationToken)
    {
        
        return request.NewState;
    }
}