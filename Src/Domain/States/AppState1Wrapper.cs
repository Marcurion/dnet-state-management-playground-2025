namespace Domain.States;

public class AppState1Wrapper : IAppState1Wrapper<List<string>>
{
    public IAppState1<List<string>> CurrentState { get; set; }
    public Action<IAppState1<List<string>>> StateChanged { get; set; }

    public AppState1Wrapper(IAppState1<List<string>> initialState)
    {
        CurrentState = initialState;
    }
}