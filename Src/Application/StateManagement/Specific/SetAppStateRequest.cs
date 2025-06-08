using Application.StateManagement.Generic;
using Domain.States;

namespace Application.StateManagement.Specific;

public class SetAppStateRequest<T> : AppStateModificationRequest<T>
{
    public AppState<T> NewState { get; set; }
}