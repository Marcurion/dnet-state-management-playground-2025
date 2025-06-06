using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Generic;

public class AppState1Request : IRequest<IAppState1>
{
    internal IAppState1 InternalLatestState { get; set; }
}