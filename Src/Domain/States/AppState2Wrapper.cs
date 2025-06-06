namespace Domain.States;

public class AppState2Wrapper : IAppState2Wrapper
{
    public IAppState2 CurrentState { get; set; }
    public Action<IAppState2> StateChanged { get; set; }

    public AppState2Wrapper(IAppState2 initialState)
    {
        CurrentState = initialState;
    }
}