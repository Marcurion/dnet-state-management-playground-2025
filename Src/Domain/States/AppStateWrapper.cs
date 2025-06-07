namespace Domain.States;

public class AppStateWrapper<T> : IAppStateWrapper<T>
{
    public IAppState<T> CurrentState { get; set; }
    public Action<IAppState<T>> StateChanged { get; set; }

    public AppStateWrapper(IAppState<T> initialState)
    {
        CurrentState = initialState;
    }
}