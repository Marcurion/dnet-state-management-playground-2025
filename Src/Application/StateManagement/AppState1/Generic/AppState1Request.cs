using Domain.States;
using MediatR;

namespace Application.StateManagement.AppState1.Generic;

public class AppState1Request<T> : IRequest<IAppState1<T>>
{
    internal IAppState1<T> InternalLatestState { get; set; }
}