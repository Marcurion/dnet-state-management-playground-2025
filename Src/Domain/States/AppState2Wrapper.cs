namespace Domain.States;

public class AppState2Wrapper : IAppState1Wrapper<List<int>>
{
    public IAppState1<List<int>> CurrentState { get; set; }
    public Action<IAppState1<List<int>>> StateChanged { get; set; }

    public AppState2Wrapper(IAppState1<List<int>> initialState)
    {
        CurrentState = initialState;
    }
}