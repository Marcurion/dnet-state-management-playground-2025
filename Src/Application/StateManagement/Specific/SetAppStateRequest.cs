using Application.StateManagement.Generic;
using Domain.States;

namespace Application.StateManagement.Specific;

public class SetAppStateRequest<T> : AppStateModificationRequest<T>
{
    public IAppState<T> NewState { get; set; }
}