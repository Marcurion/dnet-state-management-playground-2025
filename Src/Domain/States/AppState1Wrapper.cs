namespace Domain.States;

public class AppState1Wrapper : IAppState1Wrapper
{
    public IAppState1 CurrentState { get; set; }
    public Action<IAppState1> StateChanged { get; set; }

    public AppState1Wrapper(IAppState1 initialState)
    {
        CurrentState = initialState;
    }
}