using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState2.Generic;

public class AppState2Request : IRequest<IAppState2>
{
    internal IAppState2 InternalLatestState { get; set; }
}