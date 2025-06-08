using Domain.States;

namespace Application.StateManagement.Common;

public class AppStateWrapper<T> : IAppStateWrapper<T>
{
    public AppState<T> CurrentState { get; internal set; }
    public Action<AppState<T>> StateChanged { get; set; }

    public AppStateWrapper(AppState<T> initialState)
    {
        CurrentState = initialState;
    }
}