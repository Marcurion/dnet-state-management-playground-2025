namespace Domain.States;

public class AppState1Wrapper : IAppStateWrapper<List<string>>
{
    public IAppState<List<string>> CurrentState { get; set; }
    public Action<IAppState<List<string>>> StateChanged { get; set; }

    public AppState1Wrapper(IAppState<List<string>> initialState)
    {
        CurrentState = initialState;
    }
}