using Domain.States;
using MediatR;

namespace Application.StateManagement.Generic;

public class AppStateRequest<T> : IRequest<IAppState<T>>
{
    internal IAppState<T> InternalLatestState { get; set; }
}