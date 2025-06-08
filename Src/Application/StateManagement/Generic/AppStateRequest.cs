using Domain.States;
using MediatR;

namespace Application.StateManagement.Generic;

public class AppStateRequest<T> : IRequest<AppState<T>>
{
    internal AppState<T> InternalLatestState { get; set; }
}