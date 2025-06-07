namespace Domain.States;

public class AppState2Wrapper : IAppStateWrapper<List<int>>
{
    public IAppState<List<int>> CurrentState { get; set; }
    public Action<IAppState<List<int>>> StateChanged { get; set; }

    public AppState2Wrapper(IAppState<List<int>> initialState)
    {
        CurrentState = initialState;
    }
}