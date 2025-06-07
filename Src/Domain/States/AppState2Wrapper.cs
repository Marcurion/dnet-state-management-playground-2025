namespace Domain.States;

public class AppState2Wrapper : IAppState2Wrapper
{
    public AppState2 CurrentState { get; set; }
    public Action<AppState2>? StateChanged { get; set; }

    public AppState2Wrapper(AppState2 initialState)
    {
        CurrentState = initialState;
    }
}