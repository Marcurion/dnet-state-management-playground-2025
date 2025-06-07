namespace Domain.States;

public class AppState1Wrapper : IAppState1Wrapper
{
    public AppState1 CurrentState { get; set; }
    public Action<AppState1>? StateChanged { get; set; }

    public AppState1Wrapper(AppState1 initialState)
    {
        CurrentState = initialState;
    }
}