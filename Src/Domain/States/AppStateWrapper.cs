namespace Domain.States;

public class AppStateWrapper : IAppStateWrapper
{
    public AppStateWrapper(IAppState state)
    {
        CurrentState = state;
    }
    public IAppState CurrentState { get; set; }
}