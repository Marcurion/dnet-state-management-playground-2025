namespace Domain.States;

public class AppStateWrapper<T> : IAppStateWrapper<T>
{
    public AppState<T> CurrentState { get; set; }
    public Action<AppState<T>> StateChanged { get; set; }

    public AppStateWrapper(AppState<T> initialState)
    {
        CurrentState = initialState;
    }
}